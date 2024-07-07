using System;
using System.Data.SqlClient;

namespace Banking_System
{
    internal class Program 
    { 

        SqlConnection conn = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True;");
        SqlCommand cmd;

        //Register Method
        void Register()
        {
            try
            {
                Console.WriteLine("Enter your Full Name: ");
                string fname = Console.ReadLine();
                Console.WriteLine("Enter your Email: ");
                string email = Console.ReadLine();
                Console.WriteLine("Enter your Bank account Number: ");
                string b_acc = Console.ReadLine();
                Console.WriteLine("Enter your Password: ");
                string pass = Console.ReadLine();

                conn.Open();
                string query = "insert into banking values ('" + fname + "','" + email + "','" + pass + "','" + b_acc + "')";
                cmd = new SqlCommand(query, conn);
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    Console.WriteLine("Registered Successfully.");
                }
                else
                {
                    Console.WriteLine("Registration Failed.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something Gone Wrong " + ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            Login_menu();
        }

        //Login Method
        void Login()
        {
            try
            {
                Console.WriteLine("Enter your Email: ");
                string email = Console.ReadLine();
                Console.WriteLine("Enter your Password for this application: ");
                string pass = Console.ReadLine();
                conn.Open();
                string query = "select * from banking where email = @Email AND pass = @Password";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", pass);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine("Login Successful.");
                    mainMenu(reader["b_acc"].ToString());
                }
                else
                {
                    Console.WriteLine("Invalid Email or Password.");
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

            }
        }

        //First Menu for Login and Registration
        void Login_menu()
        {
            
            Console.Write("******* Welcome To Banking Application *******\n");
            Console.WriteLine("1. Register\n2. Login");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.WriteLine("**** Welcome to Register Page ****");
                    Register();
                    break;
                case 2:
                    Console.WriteLine("**** Welcome to Login Page ****");
                    Login();
                    break;
                default:
                    Console.WriteLine("Invalid Choice");
                    break;
            }
        }

        //**Main Menu**//
        void mainMenu(string b_acc)
        {
            while (true)
            {
                
                Console.WriteLine("***** This is Main Menu *****");
                Console.WriteLine("1. Add Money\n2. Withdraw Money\n3. View Balance\n4. User Details\n5. Send Money\n6. Logout");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        addMoney(b_acc);
                        break;
                    case 2:
                        withdrawMoney(b_acc);
                        break;
                    case 3:
                        viewBalance(b_acc);
                        break;
                    case 4:
                        viewUserDetails(b_acc);
                        break;
                    case 5:
                        sendMoney(b_acc);
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // 1 **ADD Money**//
        void addMoney(string b_acc)
        {
            try
            {
                Console.WriteLine("Enter your Amount: ");
                int amt = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter text message (if any): ");
                string msg = Console.ReadLine();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                //string query = "insert into Transaction values('"+msg+"',"+amt+",'"+b_acc+"')";
                //cmd=new SqlCommand(query,conn);
                string query = "INSERT INTO [Transaction] (message, amount, b_acc) VALUES ('" + msg + "', " + amt + ", '" + b_acc + "')";
                cmd = new SqlCommand(query, conn);
                //cmd.Parameters.AddWithValue("@msg", msg);
                //cmd.Parameters.AddWithValue("@amt", amt);
                //cmd.Parameters.AddWithValue("@b_acc", b_acc);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    Console.WriteLine("Money Deposited");
                }
                else
                {
                    Console.WriteLine("Failed to Deposite Money");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something gone wrong " + ex);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // 2 **Withdraw Money**//
        void withdrawMoney(string b_acc)
        {
            try
            {
                Console.WriteLine("Enter your Amount: ");
                int amt = Convert.ToInt32(Console.ReadLine());
                string msg = "Withdrawal";
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                string query = "INSERT INTO [Transaction] (message, amount, b_acc) VALUES ('" + msg + "', @Amount, @AccountNumber)";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Amount", -amt);
                cmd.Parameters.AddWithValue("@AccountNumber", b_acc);
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    Console.WriteLine("Money Withdrawal Successfull");
                }
                else
                {
                    Console.WriteLine("Failed to Withdraw Money");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something gone wrong.." + ex.Message);
            }
        }

        // 3 **View Balance**//
        void viewBalance(string b_acc)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                string query = "SELECT SUM(amount) FROM [Transaction] WHERE b_acc = @AccountNumber";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNumber", b_acc);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    Console.WriteLine("Current Balance: " + result.ToString());
                }
                else
                {
                    Console.WriteLine("Failed to retrieve balance.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // 4 **See User Details**//
          void viewUserDetails(string b_acc)
          {
              try
              {
                  if (conn.State == System.Data.ConnectionState.Open)
                  {
                      conn.Close();
                  }
                  conn.Open();
                  string query = "SELECT * FROM banking WHERE b_acc = @AccountNumber";
                  cmd = new SqlCommand(query, conn);
                  cmd.Parameters.AddWithValue("@AccountNumber", b_acc);

                  SqlDataReader reader = cmd.ExecuteReader();
                  if (reader.Read())
                  {
                      Console.WriteLine("User Details:");
                      Console.WriteLine("Full Name: " + reader["fname"].ToString());
                      Console.WriteLine("Email: " + reader["email"].ToString());
                      Console.WriteLine("Bank Account Number: " + reader["b_acc"].ToString());
                  }
                  else
                  {
                      Console.WriteLine("Failed to retrieve user details.");
                  }
                  reader.Close();
              }
              catch (Exception ex)
              {
                  Console.WriteLine("Something went wrong " + ex.Message);
              }
              finally
              {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
              }

        }

        // 5 **Send Money to other user **//
        void sendMoney(string b_acc)
        {
            try
            {
                Console.WriteLine("Enter the recipient's Bank Account Number: ");
                string to_b_acc = Console.ReadLine();
                Console.WriteLine("Enter the Amount: ");
                int amt = Convert.ToInt32(Console.ReadLine());
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();

                string checkRecipientQuery = "SELECT COUNT(*) FROM banking WHERE b_acc = @RecipientAccount";
                cmd = new SqlCommand(checkRecipientQuery, conn);
                cmd.Parameters.AddWithValue("@RecipientAccount", to_b_acc);
                int recipientExists = (int)cmd.ExecuteScalar();

                if (recipientExists == 0)
                {
                    Console.WriteLine("Recipient account not found.");
                    return;
                }

                string withdrawQuery = "INSERT INTO [Transaction] (Message, amount, b_acc) VALUES ('Transfer to " + to_b_acc + "', @Amount, @AccountNumber)";
                cmd = new SqlCommand(withdrawQuery, conn);
                cmd.Parameters.AddWithValue("@Amount", -amt);
                cmd.Parameters.AddWithValue("@AccountNumber", b_acc);
                cmd.ExecuteNonQuery();

                
                string depositQuery = "INSERT INTO [Transaction] (Message, amount, b_acc) VALUES ('Transfer from " + b_acc + "', @Amount, @RecipientAccount)";
                cmd = new SqlCommand(depositQuery, conn);
                cmd.Parameters.AddWithValue("@Amount", amt);
                cmd.Parameters.AddWithValue("@RecipientAccount", to_b_acc);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Money sent successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something Went Wrong " + ex.Message);
            }
       
        }
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Login_menu();
        }
    }
}
