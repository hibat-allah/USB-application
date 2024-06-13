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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace AppService
{
    public partial class PasswordValidationForm : Form
    {
        private HashSet<string> commonPasswords;

        public PasswordValidationForm()
        {
            InitializeComponent();
            InitializeCommonPasswords();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += PasswordValidationForm_FormClosing;
        }

        private void InitializeCommonPasswords()
        {
            commonPasswords = new HashSet<string>
            {
                "123456", "password", "123456789", "12345678", "12345", "1234567", "1234567890", "qwerty",
                "abc123", "password1", "111111", "123123", "admin", "letmein", "welcome", "monkey", "1234",
                "1q2w3e4r", "sunshine", "123", "iloveyou", "654321", "666666", "qwertyuiop", "superman",
                "asdfghjkl", "trustno1", "hello", "123321", "123qwe", "passw0rd", "password123", "000000",
                "qwerty123", "1qaz2wsx", "123654", "zaq12wsx", "dragon", "michael", "football", "baseball",
                "jennifer", "nicole", "daniel", "brittany", "michelle", "tigger", "summer", "computer",
                "princess", "matthew", "alejandro", "shadow", "ashley", "fuckyou", "jesus", "superstar",
                "monica", "pepper", "freedom", "batman", "master", "mustang", "access", "ginger", "maverick",
                "phoenix", "cowboy", "qazwsx", "zxcvbnm", "butterfly", "xoxo", "cookie",
                "sunflower", "rockstar", "yankees", "cheerleader", "flower", "internet", "captain",
                "mercedes", "arsenal", "dolphin", "eagle1", "cookie123", "matrix", "tiger", "redsox",
                "baby", "joshua", "shadow1", "apple", "scooby", "skater", "football1",
                "bubbles", "guitar", "awesome", "bananas", "soccer", "angel", "angel1", "charlie",
                "azertyuiop", "azerty", "motdepasse", "princesse", "soleil", "admin",
                "azerty1", "azert123", "12345azerty", "bonjour123", "soleil123", "princesse1", "azerty1234", "bonjour01",
                "soleil1234", "motdepasse1", "1234567890", "iloveyou1", "jeanmichel", "bonjour00",
                "azerty12345", "bonjour02", "azertyuiop1", "soleil12345", "motdepasse12",
                "azertyuiop2", "bonjour03", "motdepasse123", "bonjour04",
                "soleil123456", "azertyuiop3", "motdepasse1234", "bonjour05",
                "azertyuiop4", "bonjour06", "soleil1234567","azertyuiop5", "motdepasse123456", "azertyuiop6", "bonjour07",
                "soleil12345678", "azertyuiop7", "motdepasse1234567", "azertyuiop8", "bonjour08", "azertyuiop9",
                "motdepasse12345678", "bonjour09", "soleil123456789", "azertyuiop0", "bonjour10",
                "soleil1234567890", "azertyuiop01","azertyuiop02", "bonjour11", "azertyuiop03",
                "bonjour12","azertyuiop04","bonjour13","azertyuiop05","bonjour14","azertyuiop06",
                "bonjour15", "azertyuiop07", "soleil12345678", "bonjour16", "motdepasse12345",
                "azertyuiop08", "soleil1234567890", "bonjour17", "motdepasse1234567", "password20",
                "Azerty123!", "Motdepasse1!", "Bonjour123!", "Paris2020!", "Bienvenue1!", "Amour1234!", "Football10!",
                "Chocolat1@", "Etoile1234*", "Papillon1@", "Soleil2021!", "Automne1@", "Printemps1*", "Montagne1@",
                "Vacances1!", "Voyage2021!", "Famille1@", "Amis2020!", "Bonheur1@", "Courage2021!", "Esprit2021!",
                "Riviere1@", "Chanson1*", "Poisson1@", "Guitare1!", "Cafedu123!", "Croissant1@", "Boulangerie1@",
                "Merveille1@", "Nature2021!", "Bourbon1@", "Velours1*", "Ecologie1@", "Planete1@", "Papillon1@",
                "Etoilefilante1@", "Bretagne1@", "Normandie1@", "Provence1@", "Méditerranée1@",
                "Alpes2020!", "Pyrenees1@", "Savoie2021!", "Lorraine1@", "Bordeaux2021!", "Toulouse1@", "Marseille1@",
                "Lyon2020!", "Nantes2021@", "Nice2021!", "Lille2020@", "Strasbourg1@", "Rennes2021@", "Grenoble1@",
                "Dijon2021@", "Limoges1@", "Angers2021@", "Rouen2020@", "Clermont2021@", "Caen2020@", "Reims2021@",
                "LeHavre2020@", "Amiens2021@", "Tours2020@", "Perpignan1@", "Orleans2021@", "Metz2020@", "Besancon1@",
                "Mulhouse2021@", "Douai2021@", "Roubaix1@", "LeMans2021@", "Nancy2020@", "Cannes2021@", "Antibes2020@",
                "Valence2021@", "Blois2020@", "Chateauroux2021@", "Narbonne1@", "Brive2021@", "Pau2020@", "Bayonne2021@",
                "Tarbes2020@", "Albi2021@", "Montelimar2020@", "Chalons2021@", "Saumur2020@", "LaRochelle2021@",
                "SaintMalo2020@", "Biarritz2021@", "Vichy2020@", "Chambery2021@", "Annecy2020@", "Frejus2021@",
                "Valence2020@", "Bourg1@", "Aurillac2021@", "Vannes2020@", "Carcassonne2021@", "Perigueux2020@",
                "Colmar2021@", "SaintEtienne1@", "Macon2020@"
            };

        }

        private bool IsValidPassword(string password, string username)
        {
            if (password.Length < 8)
            {
                return false;
            }

            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            if (!(hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar))
            {
                return false;
            }

            if (password.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return false;
            }

            if (commonPasswords.Contains(password))
            {
                return false;
            }

            return true;
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            string password = passwordTextBox.Text;
            string confirmPassword = confirmPasswordTextBox.Text;
            string username = Environment.UserName; // Replace with the actual username

            if (password != confirmPassword)
            {
                messageLabel.Text = "Passwords do not match. Please try again.";
                passwordTextBox.Clear();
                confirmPasswordTextBox.Clear();
                return;
            }
            if (IsValidPassword(password, username))
            {
                MessageBox.Show("Password is valid!");
                BD.UpdateUserPassword(username, password);
                //UpdateFirstLoginFlag(username);
                this.FormClosing -= PasswordValidationForm_FormClosing; // Allow the form to close
                this.Close();
            }
            else
            {
                messageLabel.Text = "Invalid password. Please try again.";
                passwordTextBox.Clear();
                confirmPasswordTextBox.Clear();
            }
        }

       private void PasswordValidationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Prevent the form from closing
                MessageBox.Show("You must enter a valid password to proceed.");
            }
        }

        private void PasswordValidationForm_Load(object sender, EventArgs e)
        {

        }
    }

}
