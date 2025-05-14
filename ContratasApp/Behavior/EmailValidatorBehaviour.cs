using System.Text.RegularExpressions;

namespace ContratasApp.Behavior;

public class EmailValidatorBehaviour
{
    /// <summary>
    /// Behavior to validate email format in an Entry.
    /// </summary>
    public class EmailValidationBehavior : Behavior<Entry>
    {
        // Simple regex for email validation
        static readonly Regex _emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        // Bindable property to expose validity
        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(
                nameof(IsValid),
                typeof(bool),
                typeof(EmailValidationBehavior),
                false);

        /// <summary>
        /// True when the email matches the pattern
        /// </summary>
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

            var text = e.NewTextValue?.Trim() ?? string.Empty;

            // Validate email with regex
            bool ok = _emailRegex.IsMatch(text);
            IsValid = ok;

            // Visual feedback: red if invalid, default otherwise
            entry.TextColor = ok ? Colors.Black : Colors.Red;
        }
    }

}