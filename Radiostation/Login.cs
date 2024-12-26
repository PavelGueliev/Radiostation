using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Radiostation
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void dj_button_Click(object sender, EventArgs e)
        {
            FormDJ formDJ = new FormDJ();
            formDJ.FormClosed += (s, args) => this.Show();
            formDJ.Show();
            this.Hide();
        }

        private void manager_button_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-6MQUQFM\\SQLEXPRESS;Initial Catalog=Radiostation;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            string login = txtLogin.Text;
            string password = txtPassword.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                                SELECT u.UserID, u.Role,
                                       COALESCE(p.ФИО, m.ФИО) AS UserName
                                FROM Users_radiostation u
                                LEFT JOIN Presenter p ON u.PresenterID = p.КодВедущего
                                LEFT JOIN Manager m ON u.ManagerID = m.КодМенеджера
                                WHERE u.Логин = @Login AND u.Пароль = @Password;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", password);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblError.Text = "";
                        // Получаем имя пользователя и роль
                        string userName = reader["UserName"].ToString();
                        string role = reader["Role"].ToString();
                        Program.CurrentUsername = userName;
                        MessageBox.Show($"Добро пожаловать, {userName}!", "Вход выполнен");

                        // Открытие формы в зависимости от роли
                        switch (role)
                        {
                            case "Presenter":
                                FormDJ formDJ = new FormDJ();
                                formDJ.FormClosed += (s, args) => this.Show();
                                formDJ.Show();
                                this.Hide();
                                break;

                            case "Manager":
                                FormManager formManager = new FormManager();
                                formManager.FormClosed += (s, args) => this.Show();
                                formManager.Show();
                                this.Hide();
                                break;
                        }

                        this.Hide(); // Скрываем форму входа
                    }
                    else
                    {
                        lblError.Text = "Неверный логин или пароль.";
                    }
                }
            }
        }

        private void visitor_button_Click(object sender, EventArgs e)
        {
            Guest guest = new Guest();
            guest.FormClosed += (s, args) => this.Show();
            guest.Show();
            this.Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
