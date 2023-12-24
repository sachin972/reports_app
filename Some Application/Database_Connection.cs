using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Some_Application
{
    internal class Database_Connection
    {
        // Method to open the database connection
        public static void OpenDatabaseConnection(string connectionString)
        {
            //CompleteReport completeReport = new CompleteReport();
            //OleDbConnection connection = new OleDbConnection(connectionString);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database connection opened successfully.");


                    // Perform database operations here after opening the connection
                    // Example: Execute queries, retrieve data, etc.
                    string query = "SELECT * FROM Divmas"; // Replace YourTable with your table name

                    // Create a command object
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Execute the query and read data
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data using reader
                                string[] id = new string[40];

                                for (int i = 0; i < reader.FieldCount; i++)
                                {

                                    //if (i == 1)
                                    //{
                                    //    completeReport.divName = reader.GetValue(i) as string;
                                    //}
                                    //if (i == 3)
                                    //{
                                    //    completeReport.divPhone1 = reader.GetValue(i) as string;
                                    //}
                                    //if (i == 4)
                                    //{
                                    //    completeReport.divPhone2 = reader.GetValue(i) as string;
                                    //}
                                    //if (i == 16)
                                    //{
                                    //    completeReport.divCommission = reader.GetValue(i) as string;
                                    //}
                                }
                            }
                        }
                    }

                    string query2 = @"SELECT *
FROM buyermas, sglmas
WHERE buyermas.buyercd = sglmas.sglcode;";



                    using (OleDbCommand command2 = new OleDbCommand(query2, connection))
                    {
                        // Execute the query and read data
                        using (OleDbDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data using reader
                                //string[] id = new string[500];
                                //Console.WriteLine(reader.GetValue(0) + "hello");

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    //Console.WriteLine(reader.GetValue(i) + "hello");
                                }
                            }
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    // Handle exceptions related to database connection
                }

            }
        }

        public void CloseConnection(OleDbConnection connection)
        {
            // Ensure the connection is closed when done
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public static List<string> getDivData(string connectionString)
        {
            List<string> divData = new List<string>();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database connection opened successfully.");


                    // Perform database operations here after opening the connection
                    // Example: Execute queries, retrieve data, etc.
                    string query = "SELECT * FROM Divmas"; // Replace YourTable with your table name

                    // Create a command object
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Execute the query and read data
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data using reader
                                string[] id = new string[40];

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (i == 0)
                                    {
                                        divData.Add(reader.GetValue(i) as string);
                                    }
                                    if (i == 1)
                                    {
                                        divData.Add(reader.GetValue(i) as string);
                                    }
                                    if (i == 3)
                                    {
                                        divData.Add(reader.GetValue(i) as string);
                                    }
                                    if (i == 4)
                                    {
                                        divData.Add(reader.GetValue(i) as string);
                                    }
                                    if (i == 16)
                                    {
                                        divData.Add(reader.GetValue(i) as string);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    // Handle exceptions related to database connection
                }

            }
            return divData;
        }

        public static List<List<string>> getBuyersList(string connectionString)
        {
            List<List<string>> buyersList = new List<List<string>>();

            //for (int i = 0; i < 50; i++)
            //{
            //    buyersList[i] = new string[30];
            //}
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database connection opened successfully.");


                    // Perform database operations here after opening the connection
                    // Example: Execute queries, retrieve data, etc.

                    string query2 = @"SELECT bpurcha, buyercd
FROM buyermas;";

                    using (OleDbCommand command2 = new OleDbCommand(query2, connection))
                    {
                        // Execute the query and read data
                        using (OleDbDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data using reader
                                List<string> list = new List<string>();

                                for (int i = 0; i < 2; i++)
                                {

                                    if (i != 0)
                                    {
                                        list.Add(reader.GetValue(i) as string);
                                        //Console.WriteLine(value: (reader.GetValue(i) as string));
                                    }
                                    else
                                    {
                                        list.Add(reader.GetInt32(i).ToString());
                                        //Console.WriteLine(value: (reader.GetInt32(i)));
                                    }
                                }
                                buyersList.Add(list);
                            }
                        }
                    }
                    return buyersList;
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    // Handle exceptions related to database connection
                    return buyersList;
                }

            }
        }

        public static List<string> getBuyerDetails(string connectionString, string buyerCode)
        {
            List<string> buyerDetails = new List<string>();
            //Console.WriteLine(buyerCode);


            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database connection opened successfully.");


                    // Perform database operations here after opening the connection
                    // Example: Execute queries, retrieve data, etc.

                    string query2 = @"SELECT sgldesc, sgladd1, sgladd2 FROM sglmas
                        WHERE sglmas.sglcode=@buyerCode;";
                    OleDbCommand command = new OleDbCommand(query2, connection);
                    command.Parameters.AddWithValue("@buyerCode", buyerCode);
                    using (command)
                    {
                        // Execute the query and read data
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data using reader

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    buyerDetails.Add(reader.GetValue(i) as string);
                                }
                            }
                        }
                    }

                    query2 = @"SELECT buyerdt from Buyermas
                        WHERE Buyermas.buyercd=" + buyerCode + ";";
                    OleDbCommand command2 = new OleDbCommand(query2, connection);
                    command2.Parameters.AddWithValue("@buyerCode", buyerCode);
                    using (command2)
                    {
                        // Execute the query and read data
                        using (OleDbDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data using reader

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    buyerDetails.Add(reader.GetValue(i).ToString());
                                    //Console.WriteLine(reader.GetValue(i));
                                }
                            }
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    // Handle exceptions related to database connection
                }

            }
            return buyerDetails;
        }

        public static List<string> getColumns(string connectionString)
        {
            List<string> columns = new List<string>();



            return columns;
        }

        public static List<List<object>> getItemDetails(string connectionString, int purchaCode)
        {
            List<List<object>> items = new List<List<object>>();
            //Console.WriteLine(purchaCode);

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database connection opened successfully.");


                    // Perform database operations here after opening the connection
                    // Example: Execute queries, retrieve data, etc.

                    string code = purchaCode.ToString();

                    string query2 = @"SELECT * FROM Buyerdet, Dgrfile1, Itemmas, Grmas WHERE (Buyerdet.bdpurcha=@purchaCode) AND (Buyerdet.bdgrsno = Dgrfile1.dgr1srno) AND (Dgrfile1.dgr1icd = Itemmas.itemcode) AND (Dgrfile1.dgr1srno = Grmas.grsno)";
                    OleDbCommand command = new OleDbCommand(query2, connection);

                    // Assuming purchaCode is a string or an appropriate data type
                    command.Parameters.AddWithValue("@purchaCode", code);

                    using (command)
                    {
                        // Execute the query and read data
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data using reader
                                List<object> list = new List<object>();
                                bool flag = false;
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.WriteLine($"{i} {reader.GetValue(i)}");
                                    if (i == 46)
                                    {
                                        object x = reader.GetValue(i);
                                        for (int j = 0; j < items.Count; j++)
                                        {
                                            if (items[j].IndexOf(x) >= 0 && items[j].IndexOf(list[7]) >= 0)
                                            {
                                                flag = true;
                                                break;
                                            }
                                        }
                                        list.Add(reader.GetValue(i) as string);
                                        if (flag)
                                        {
                                            break;
                                        }
                                    }
                                    else if (i == 5)
                                    {
                                        list.Add(reader.GetValue(i) as object);
                                    }
                                    else if (i == 6)
                                    {
                                        list.Add(reader.GetValue(i) as object);
                                    }
                                    else if (i == 9)
                                    {
                                        list.Add(reader.GetValue(i) as object);
                                    }
                                    else if (i == 7)
                                    {
                                        list.Add(reader.GetValue(i) as object);
                                    }
                                    else if (i == 21)
                                    {
                                        list.Add(reader.GetValue(i) as object);
                                    }
                                    else if (i == 40)
                                    {
                                        list.Add(reader.GetValue(i) as string);
                                    }
                                    else if (i == 41)
                                    {
                                        list.Add(reader.GetValue(i) as string);
                                    }
                                    else if(i == 67)
                                    {
                                        list.Add(reader.GetValue(i) as string);
                                    }
                                    else if(i == 66)
                                    {
                                        list.Add(reader.GetValue(i) as string);
                                    }
                                    else
                                    {
                                        list.Add(reader.GetValue(i));
                                    }
                                    
                                    //list.Add(reader.GetValue(i));

                                    //Console.WriteLine(reader.GetValue(i));
                                }
                                if (flag)
                                {
                                    continue;
                                }

                                items.Add(list);
                            }
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    // Handle exceptions related to database connection
                }

            }

            items = items.GroupBy(list => string.Join(",", list))
                                              .Select(group => group.First())
                                              .ToList();

            return items;
        }
        public static int getRemainingBalance(string connectionString, string buyerCode)
        {
            int remBalance = 0;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database connection opened successfully.");


                    // Perform database operations here after opening the connection
                    // Example: Execute queries, retrieve data, etc.

                    string query2 = @"SELECT sgllybal
FROM sglbal WHERE sglsglcd=" + buyerCode + ";";
                    OleDbCommand command2 = new OleDbCommand(query2, connection);

                    command2.Parameters.AddWithValue("@buyerCode", buyerCode);

                    using (command2)
                    {
                        // Execute the query and read data
                        using (OleDbDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data using reader
                                Console.WriteLine(reader.GetValue(0).ToString());
                            }
                        }
                    }

                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    // Handle exceptions related to database connection

                }

            }

            return remBalance;
        }

        public static List<List<string>> sellersList(string connectionString)
        {
            List<List<string>> buyersList = new List<List<string>>();

            //for (int i = 0; i < 50; i++)
            //{
            //    buyersList[i] = new string[30];
            //}
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database connection opened successfully.");


                    // Perform database operations here after opening the connection
                    // Example: Execute queries, retrieve data, etc.

                    string query2 = @"SELECT bpurcha, buyercd
FROM buyermas;";

                    using (OleDbCommand command2 = new OleDbCommand(query2, connection))
                    {
                        // Execute the query and read data
                        using (OleDbDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data using reader
                                List<string> list = new List<string>();

                                for (int i = 0; i < 2; i++)
                                {

                                    if (i != 0)
                                    {
                                        list.Add(reader.GetValue(i) as string);
                                        //Console.WriteLine(value: (reader.GetValue(i) as string));
                                    }
                                    else
                                    {
                                        list.Add(reader.GetInt32(i).ToString());
                                        //Console.WriteLine(value: (reader.GetInt32(i)));
                                    }
                                }
                                buyersList.Add(list);
                            }
                        }
                    }
                    return buyersList;
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    // Handle exceptions related to database connection
                    return buyersList;
                }

            }
        }
    }
}
