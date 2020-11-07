using System;
using System.Collections.Generic;
using Human_Resource_Information_System.Model;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Human_Resource_Information_System.Database
{
    class SchoolDBAdapter
    {
        private const string db = "";
        private const string user = "";
        private const string pass = "";
        private const string server = "";

        private readonly MySqlConnection conn;
        public SchoolDBAdapter()
        {
            string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
            conn = new MySqlConnection(connectionString);
        }

        public List<Staff> FetchBasicStaffDetails()
        {
            MySqlDataReader rdr = null;
            List<Staff> staff = new List<Staff>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select id, given_name, family_name, title, category, photo from staff", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    staff.Add(new Staff()
                    {
                        ID = rdr.GetInt32("id"),
                        Title = rdr.GetString("title"),
                        Photo = rdr.GetString("photo"),
                        Category = (Category)Enum.Parse(typeof(Category), rdr.GetString("category")),
                        FamilyName = rdr.GetString("family_name"),
                        GivenName = rdr.GetString("given_name")
                    });
                }
            }
            catch
            {
                //If we can't connect to DB, display error and close the application
                MessageBox.Show("Could not connect to database, ensure you're connected to the UTAS VPN.");
                Environment.Exit(0);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return staff;
        }

        public Staff FetchStaffDetails(int staffID)
        {
            Staff staff = new Staff();
            MySqlDataReader rdr = null;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select id, given_name, family_name, title, category, photo, phone, room, email, campus from staff where id=@staffID", conn);
                cmd.Parameters.AddWithValue("@staffID", staffID.ToString());
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    staff = new Staff()
                    {
                        ID = rdr.GetInt32("id"),
                        Title = rdr.GetString("title"),
                        Phone = rdr.GetString("phone"),
                        Email = rdr.GetString("email"),
                        Room = rdr.GetString("room"),
                        Photo = rdr.GetString("photo"),
                        Campus = (Campus)Enum.Parse(typeof(Campus), rdr.GetString("campus")),
                        Category = (Category)Enum.Parse(typeof(Category), rdr.GetString("category")),
                        FamilyName = rdr.GetString("family_name"),
                        GivenName = rdr.GetString("given_name")
                    };
                }
            }
            catch
            {
                //If we can't connect to DB, display error and close the application
                MessageBox.Show("Could not connect to database, ensure you're connected to the UTAS VPN.");
                Environment.Exit(0);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            staff.Consultations = FetchConsultationTimes(staff.ID);
            staff.TeachingUnits = FetchTeachingUnits(staff.ID);
            return staff;
        }

        public List<Event> FetchConsultationTimes(int staffID)
        {
            List<Event> consultationTimes = new List<Event>();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select day, start, end from consultation where staff_id=@staffID", conn);
                cmd.Parameters.AddWithValue("@staffID", staffID.ToString());
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), rdr.GetString("day"));
                    Time start = new Time(rdr.GetString("start"));
                    Time end = new Time(rdr.GetString("end"));
                    Event consultationTime = new Event()
                    {
                        Day = day,
                        Start = start,
                        End = end
                    };

                    consultationTimes.Add(consultationTime);
                }
            }
            catch
            {
                //If we can't connect to DB, display error and close the application
                MessageBox.Show("Could not connect to database, ensure you're connected to the UTAS VPN.");
                Environment.Exit(0);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return consultationTimes;
        }

        public List<UnitClass> FetchTeachingUnits(int staffID)
        {
            List<UnitClass> modelTimes = new List<UnitClass>();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select class.unit_code, unit.title, class.room, class.day, class.start, class.end from class left join unit on class.unit_code = unit.code where staff=@staffID ", conn);
                cmd.Parameters.AddWithValue("@staffID", staffID.ToString());
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), rdr.GetString("day"));
                    Time start = new Time(rdr.GetString("start"));
                    Time end = new Time(rdr.GetString("end"));
                    string unitCode = rdr.GetString("unit_code");
                    string room = rdr.GetString("room");
                    string unitname = rdr.GetString("title");

                    UnitClass modelTime = new UnitClass()
                    {
                        UnitName = unitname,
                        Day = day,
                        Start = start,
                        End = end,
                        ClassUnitCode = unitCode,
                        Room = room
                    };

                    modelTimes.Add(modelTime);
                }
            }
            catch
            {
                //If we can't connect to DB, display error and close the application
                MessageBox.Show("Could not connect to database, ensure you're connected to the UTAS VPN.");
                Environment.Exit(0);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return modelTimes;
        }

        public List<Unit> FetchUnits()
        {
            MySqlDataReader rdr = null;
            List<Unit> units = new List<Unit>();
 
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT code, title, coordinator FROM unit ORDER BY code, title", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    units.Add(new Unit()
                    {
                        Code = rdr.GetString("code"),
                        Title = rdr.GetString("title"),
                        UnitCoordinatorID = rdr.GetInt32("coordinator")
                    });
                }
            }
            catch
            {
                //If we can't connect to DB, display error and close the application
                MessageBox.Show("Could not connect to database, ensure you're connected to the UTAS VPN.");
                Environment.Exit(0);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return units;
        }

        public List<UnitClass> FetchUnitClasses(string unitCode)
        {
            MySqlDataReader rdr = null;
            List<UnitClass> unitClasses = new List<UnitClass>();
 
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select unit_code, campus, day, start, end, type, room, staff from class where unit_code = @unitCode", conn);
                cmd.Parameters.AddWithValue("@unitCode", unitCode);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    unitClasses.Add(new UnitClass()
                    {
                        Day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), rdr.GetString("day")),
                        ClassUnitCode = rdr.GetString("unit_code"),
                        Room = rdr.GetString("room"),
                        Start = new Time(rdr.GetString("start")),
                        End = new Time(rdr.GetString("end")),
                        Campus = (Campus)Enum.Parse(typeof(Campus), rdr.GetString("campus")),
                        ClassType = (ClassType)Enum.Parse(typeof(ClassType), rdr.GetString("type")),
                        StaffID = rdr.GetInt32("staff")
                    });
                }
            }
            catch
            {
                //If we can't connect to DB, display error and close the application
                MessageBox.Show("Could not connect to database, ensure you're connected to the UTAS VPN.");
                Environment.Exit(0);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return unitClasses;
        }

        public List<Event> FetchAllConsultations()
        {
            List<Event> consultationTimes = new List<Event>();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select day, start, end from consultation", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), rdr.GetString("day"));
                    Time start = new Time(rdr.GetString("start"));
                    Time end = new Time(rdr.GetString("end"));

                    Event consultationTime = new Event()
                    {
                        Day = day,
                        Start = start,
                        End = end
                    };

                    consultationTimes.Add(consultationTime);
                }
            }
            catch
            {
                //If we can't connect to DB, display error and close the application
                MessageBox.Show("Could not connect to database, ensure you're connected to the UTAS VPN.");
                Environment.Exit(0);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return consultationTimes;
        }

        public List<Event> FetchAllUnitClasses()
        {
            List<Event> classTimes = new List<Event>();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select day, start, end, campus from class", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), rdr.GetString("day"));
                    Time start = new Time(rdr.GetString("start"));
                    Time end = new Time(rdr.GetString("end"));
                    Campus campus = (Campus)Enum.Parse(typeof(Campus), rdr.GetString("campus"));

                    Event classTime = new Event()
                    {
                        Day = day,
                        Start = start,
                        End = end,
                        Campus = campus
                    };

                    classTimes.Add(classTime);
                }
            }
            catch
            {
                //If we can't connect to DB, display error and close the application
                MessageBox.Show("Could not connect to database, ensure you're connected to the UTAS VPN.");
                Environment.Exit(0);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return classTimes;
        }
    }

}

