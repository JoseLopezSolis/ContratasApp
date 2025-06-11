using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

[QueryProperty(nameof(ClientId), "clientId")]
public partial class AddClientPageViewModel : BasePageViewModel
    {
        
        #region Observable properties
        
        [ObservableProperty] private int clientId;
        [ObservableProperty] private string name;
        [ObservableProperty] private string lastName;
        [ObservableProperty] private string phone;
        [ObservableProperty] private string email;
        [ObservableProperty] private string paymentMethod;
        [ObservableProperty] private string imagePath;
        [ObservableProperty] private ImageSource profileImage;
        
        #endregion

        #region Picker options
        public IList<string> PaymentMethods { get; } =
            new List<string> { "Efectivo", "Transferencia bancaria", "Ambas" };
        #endregion

        #region Services
        readonly IClientService _clientService;
        #endregion

        #region Constants
        const string DefaultImageFileName = "default_profile.png";
        private readonly string DefaultPaymentMethod = "Transferencia y efectivo" ;
        #endregion

        #region Constructor
        public AddClientPageViewModel(
            IClientService clientService,
            INavigationService navigationService) 
            : base(navigationService)
        {
            _clientService = clientService;
            ImagePath = DefaultImageFileName;
            ProfileImage = ImageSource.FromFile(DefaultImageFileName);
        }
        #endregion

        #region Relays commands

        /// <summary>
        /// Open gallery, copy the picture to AppData and update ImagePath
        /// </summary>
        [RelayCommand]
        async Task PickImageAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Selecciona una foto de perfil" });
                
                if (result == null)
                    return;

                var newFile = Path.Combine(
                    FileSystem.AppDataDirectory, result.FileName);

                // Copia el fichero
                using var src = await result.OpenReadAsync();
                using var dst = File.OpenWrite(newFile);
                await src.CopyToAsync(dst);

                ImagePath = newFile;
            }
            catch (PermissionException)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Permiso denegado",
                                  "No se puede acceder a la galería.",
                                  "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Error",
                                  $"No se pudo seleccionar la imagen: {ex.Message}",
                                  "OK");
            }
        }

        /// <summary>
        /// Guarda el cliente (incluida la ruta de la imagen) y navega atrás.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanSave))]
        async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(PaymentMethod))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Campos requeridos",
                    "Por favor completa todos los campos obligatorios: Nombre, Apellido, Teléfono y Método de pago.",
                    "Aceptar");
                return;
            }

            var client = new Client
            {
                Id = ClientId,
                Name = name,
                LastName = lastName,
                Phone = phone?.Trim(),
                Email = email, // puede ser null
                PaymentMethod = ValidPaymentMethod(PaymentMethod),
                ImagePath = ImagePath,
                IsArchived = false
            };

            needRefreshPage = true;

            if (ClientId > 0)
                await _clientService.UpdateAsync(client);
            else
                await _clientService.AddAsync(client);

            await NavigationService.GoBackAsync();
        }

        #endregion
        
        #region OnChanged methods
        partial void OnClientIdChanged(int id)
            => LoadExistingClientAsync(id);
        
        partial void OnImagePathChanged(string value)
        {
            ProfileImage = ImageSource.FromFile(value);
        }
        
        partial void OnNameChanged(string value)
            => SaveCommand.NotifyCanExecuteChanged();

        partial void OnLastNameChanged(string value)
            => SaveCommand.NotifyCanExecuteChanged();
        
        partial void OnPhoneChanged(string value)
            => SaveCommand.NotifyCanExecuteChanged();
        
        #endregion

        #region Validators
        private string ValidPaymentMethod(string paymentMethod)
        {
            if (paymentMethod.Equals(null))
                return "Ambos";
            return paymentMethod;
        }
        #endregion

        #region Extra methods

        async void LoadExistingClientAsync(int id)
        {
            var existing = await _clientService.GetByIdAsync(id);
            Name = existing.Name;
            LastName = existing.LastName;
            Phone = existing.Phone;
            Email = existing.Email;
            PaymentMethod = existing.PaymentMethod;
            ImagePath = existing.ImagePath;
        }
        
        
        bool CanSave()
            => !string.IsNullOrWhiteSpace(Name)
               && !string.IsNullOrWhiteSpace(LastName)
               && !string.IsNullOrWhiteSpace(Phone)
               && Phone.Length > 9;
        #endregion
        
    }
