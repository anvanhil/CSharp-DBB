using MySql.Data.MySqlClient;
namespace BaseDeDonnee

{
    internal class Program
    {
        static void Main(string[] args)
        {
            Connection();
            int stockEssence = 100;
            int stockGasoil = 100;


            float prixEssence = 1.97f;
            float prixGasoil = 1.80f;
            List<string> listeAchat = new List<string>();
            string retour = "non";
            while (retour != "oui")
            {
                Console.WriteLine("Bienvenue chez Mathi entreprise! Que désirez vous?");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("1-Essence // 2-Gasoil //  3-Afficher l'historique d'achat // 4-Quitter ");
                switch (Console.ReadLine())
                {

                    case "1":
                        Console.Clear();
                        Console.WriteLine("Très bon choix");
                        Console.WriteLine("Combien de litre d'Essence voulez vous? " + stockEssence + " litres disponibles //(prix:1.97/L)");
                        int achatEssence = int.Parse(Console.ReadLine());
                        Console.WriteLine("Veuillez patienter nous traitons votre commande");
                        if (achatEssence > stockEssence)
                        {
                            Console.WriteLine("Nous n'avons pas le stock nécessaire ! Veuillez saisir une autre valeur");
                            break;
                        }
                        else
                        {
                            float prixTotalEssence = achatEssence * prixEssence;
                            Console.WriteLine("Le prix total est de " + prixTotalEssence + " €! Merci de votre visite");
                            string historique = DateTime.Now + "  " + achatEssence + " litre(s) d'essences achetés" + " pour " + prixTotalEssence + " euros";
                            listeAchat.Add(historique);
                            stockEssence -= achatEssence;

                            WriteHisto("Essence",achatEssence);
                            break;
                        }

                    case "2":
                        Console.Clear();
                        Console.WriteLine("Très bon choix"); 
                        Console.WriteLine("Combien de litre de Gasoil voulez vous? " + stockGasoil + " litres disponibles // (prix:1.80/L)");
                        int achatGasoil = int.Parse(Console.ReadLine());
                        if (achatGasoil > stockGasoil)
                        {
                            Console.WriteLine("Nous n'avons pas le stock nécéssaire! Veuillez saisir une autre valeur");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Veuillez patienter nous traitons votre commande");
                            float prixTotalGasoil = achatGasoil * prixGasoil;
                            Console.WriteLine("Le prix total est de " + prixTotalGasoil + " euros" + " Merci de votre visite");
                            string historique = DateTime.Now + "  " + achatGasoil + " litre(s) de gasoil achetés" + " pour " + prixTotalGasoil + " euros";
                            listeAchat.Add(historique);
                            stockGasoil -= achatGasoil;

                            WriteHisto("Gasoil", achatGasoil);
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            LectureList();
                            break;
                        }
                    case "4":
                        {
                            Console.Clear();
                            Console.WriteLine("Au revoir et à bientôt");
                            retour = "oui";
                            break;
                        }

                    default:
                        return;
                }
            }
            
            
        }
            static void Connection()
            {
                try
                {
                    string connectionString = GetConnectionString();
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        Console.WriteLine("Connection à la BDD");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Impossible de se connecter à la BDD " + e.Message);
                }
            }
            static void LectureList()
            {
                string donnee = "";
                Console.WriteLine("Lecture historique \r\n");
                try
                {
                    string connectionString = GetConnectionString();
                    string queryString = "SELECT * FROM historiqueachat;";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(queryString, connection);
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                donnee = "";
                                for (int z = 0; z <= 3; z++)
                                {
                                    donnee = donnee + " " + reader.GetString(z);
                                }
                                Console.WriteLine(donnee);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PTDR ça bug " + ex.Message);
                }
            }

            static void WriteHisto(string carburant, double qte)
            {
                string connectionString = GetConnectionString();
                var date = DateTime.Now.ToString("yyyyMMddhhmmss");

                try
                {
                    string queryString = "INSERT historiqueachat(date,carburant,quantitee) VALUES(";

                    queryString += "'" + date + "','" + carburant + "','" + qte + "'";
                    queryString += ");";
                    Console.WriteLine(queryString);
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(queryString, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("La liste n'a pas été exportée " + e.Message);
                }
            }

            static private string GetConnectionString()
            {
                return "SERVER = localhost; DATABASE=c#; UID=root;password=;";
            }



    }
}