using System;
using System.Drawing;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class RegisterForm : Form
    {
       private readonly UserRepository _userRepository;

        public string Login { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }

        public RegisterForm()
        {
            InitializeComponent();
            _userRepository = new UserRepository();
        }

        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    var login = txtLogin.Text.Trim();
                    var password = txtPassword.Text;
                    var role = Convert.ToString(cbRole.SelectedItem);

                    await _userRepository.RegisterUserAsync(login, password, role);
                    MessageBox.Show("Регистрация успешна!");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Введите логин");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Введите пароль");
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Пароли не совпадают");
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов");
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}