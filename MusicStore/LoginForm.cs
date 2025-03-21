using System;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class LoginForm : Form
    {
        private readonly UserRepository _userRepository;

        public LoginForm()
        {
            InitializeComponent();
            _userRepository = new UserRepository();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var login = txtLogin.Text.Trim();
                var password = txtPassword.Text;

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Введите логин и пароль");
                    return;
                }

                var user = await _userRepository.GetUserAsync(login);
                if (user == null || !user.IsActive)
                {
                    MessageBox.Show("Пользователь не найден или неактивен");
                    return;
                }

                var passwordHash = Convert.ToBase64String(
                    System.Security.Cryptography.SHA256.Create()
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));

                if (passwordHash != user.PasswordHash)
                {
                    MessageBox.Show("Неверный пароль");
                    return;
                }

                // Сохраняем информацию о пользователе
                Properties.Settings.Default.CurrentUserLogin = login;
                Properties.Settings.Default.CurrentUserRole = user.Role;
                Properties.Settings.Default.Save();

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе: {ex.Message}");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            using (var registerForm = new RegisterForm())
            {
                if (registerForm.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }
            }
        }
    }
}
