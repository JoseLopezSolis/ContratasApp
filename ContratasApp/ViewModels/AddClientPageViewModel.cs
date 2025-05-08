using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContratasApp.Models;
using ContratasApp.Services.Interfaces;
using ContratasApp.ViewModels.Base;

namespace ContratasApp.ViewModels;

[QueryProperty(nameof(ClientId), "clientId")]
public partial class AddClientPageViewModel : BasePageViewModel
    {
        const string DefaultImageFileName = "default_profile.png";

        // Campos de formulario
        [ObservableProperty] private int clientId;
        [ObservableProperty] private string name;
        [ObservableProperty] private string lastName;
        [ObservableProperty] private string phone;
        [ObservableProperty] private string email;
        [ObservableProperty] private string paymentMethod;

        // Imagen de perfil
        [ObservableProperty] private string imagePath;
        [ObservableProperty] private ImageSource profileImage;

        // Opciones para el Picker
        public IList<string> PaymentMethods { get; } =
            new List<string> { "Efectivo", "Transferencia" };

        readonly IClientService _clientService;

        public AddClientPageViewModel(
            IClientService clientService,
            INavigationService navigationService) 
            : base(navigationService)
        {
            _clientService = clientService;

            // Inicializa imagen por defecto
            ImagePath = DefaultImageFileName;
            ProfileImage = ImageSource.FromFile(DefaultImageFileName);
        }
        
        // Este partial se invoca automáticamente al recibir el query param
        partial void OnClientIdChanged(int id)
            => LoadExistingClientAsync(id);
        
        async void LoadExistingClientAsync(int id)
        {
            var existing = await _clientService.GetByIdAsync(id);
            if (existing == null) return;

            // Divide nombre y apellido si los guardas juntos
            var partes = existing.Name.Split(' ', 2);
            Name = partes[0];
            LastName = partes.Length > 1 ? partes[1] : string.Empty;

            Phone = existing.Phone;
            Email = existing.Email;
            PaymentMethod = existing.PaymentMethod;
            ImagePath = existing.ImagePath;
        }

        /// <summary>
        /// Se invoca cada vez que ImagePath cambia: actualiza ProfileImage.
        /// </summary>
        partial void OnImagePathChanged(string value)
        {
            ProfileImage = ImageSource.FromFile(value);
        }

        /// <summary>
        /// Abre galería, copia foto a AppData y actualiza ImagePath.
        /// </summary>
        [RelayCommand]
        async Task PickImageAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(
                    new MediaPickerOptions { Title = "Selecciona foto de perfil" });
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
            var client = new Client
            {
                Id = ClientId,
                Name          = $"{Name} {LastName}".Trim(),
                Phone         = Phone?.Trim(),
                Email         = Email?.Trim(),
                PaymentMethod = PaymentMethod,
                ImagePath     = ImagePath,
                IsArchived = false
            };

            if (ClientId > 0)
                await _clientService.UpdateAsync(client);
            else
                await _clientService.AddAsync(client);

            await NavigationService.GoBackAsync();
        }

        bool CanSave() =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(LastName);

        partial void OnNameChanged(string value)
            => SaveCommand.NotifyCanExecuteChanged();

        partial void OnLastNameChanged(string value)
            => SaveCommand.NotifyCanExecuteChanged();
    }
