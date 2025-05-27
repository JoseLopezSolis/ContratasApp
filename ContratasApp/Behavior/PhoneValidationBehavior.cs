using System.Text.RegularExpressions;

namespace ContratasApp.Behavior;

public class PhoneValidationBehavior : Behavior<Entry>
    {
        // Allow exactly 10 digits
        static readonly Regex _phoneRegex = new(@"^\d{10}$");

        // Expose IsValid so you can bind button enabled state
        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(
                nameof(IsValid),
                typeof(bool),
                typeof(PhoneValidationBehavior),
                false);

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidProperty, value);
        }

        protected override void OnAttachedTo(Entry entry)
        {
            base.OnAttachedTo(entry);
            entry.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is not Entry entry)
                return;

            // Keep only digits
            var digits = new string(e.NewTextValue?.Where(char.IsDigit).ToArray() ?? System.Array.Empty<char>());
            
            // Limit to max 10 digits
            if (digits.Length > 10)
                digits = digits.Substring(0, 10);

            // Update the Entry text to the limited digits
            entry.TextChanged -= OnTextChanged;
            entry.Text = digits;
            entry.TextChanged += OnTextChanged;

            // Validate exact 10-digit pattern
            var ok = _phoneRegex.IsMatch(digits);
            IsValid = ok;

            // Visual feedback: red if invalid, default otherwise
            entry.TextColor = ok ? Colors.Black : Colors.Red;
        }
    }
