using System;
using MySql.Data.MySqlClient;

namespace ruyi
{
    class Location
    {
        static string sqlconstr = "Database=" + Properties.Settings.Default.database + ";Data Source=" + Properties.Settings.Default.source + ";User ID=" + Properties.Settings.Default.user + "; password=" + Properties.Settings.Default.password;
        public string country_code { get; set; }
        public string region_name { get; set; }
        public string city_name { get; set; }
        public string ip_from { get; set; }
        public string country_name { get; set; }
        public string ip_to { get; set; }
        public Location()
        {
            region_name = "LAN";
            city_name = "LAN";
            country_name = "LAN";
            country_code = "null";
            ip_from = "null";
            ip_to = "null";
        }
        public Location(string ip)  //通过IP进行初始化
        {
            
            double ipnumber = Dot2LongIP(ip); //ip转为double类型,例如1.2.3.4=1*256^3+2*256^2+3*256+4
            //通过数据库进行查询
            using (MySqlConnection thisConnection = new MySqlConnection(sqlconstr))
            {
                try
                {

                     //根据IP值的大小，分别查询对应的表格
                    string tableName = "";
                    if (ipnumber<=709157631)
                    {
                        tableName = "ip2location_1";
                    }else if(ipnumber<=1143878655)
                    {
                        tableName = "ip2location_2";
                    }else if(ipnumber<= 1310948351)
                    {
                        tableName = "ip2location_3";
                    }
                    else if (ipnumber <= 1485533695)
                    {
                        tableName = "ip2location_4";
                    }
                    else if (ipnumber <= 1670161919)
                    {
                        tableName = "ip2location_5";
                    }
                    else if (ipnumber <= 2037796095)
                    {
                        tableName = "ip2location_6";
                    }
                    else if (ipnumber <= 2921336319)
                    {
                        tableName = "ip2location_7";
                    }
                    else if (ipnumber <= 3158435839)
                    {
                        tableName = "ip2location_8";
                    }
                    else if (ipnumber <= 3449811967)
                    {
                        tableName = "ip2location_9";
                    }
                    else 
                    {
                        tableName = "ip2location_10";
                    }
                    string cmdString = "SELECT  * FROM "+tableName +" where ip_from <=" + ipnumber + " and ip_to >=" + ipnumber;
               
                thisConnection.Open();
                MySqlCommand cmd = new MySqlCommand(cmdString, thisConnection);
                
                MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())   //如果没有读到数据，全部置为空
                {
                    country_code = "null";
                    country_name = "null";
                    city_name = "null";
                    region_name = "null";
                    ip_from = "null";
                    ip_to = "null";

                    reader.Close();
                }
                else   //有数据时进行赋值
                {
                    country_code = reader["country_code"].ToString();
                    country_name = reader["country_name"].ToString();
                    region_name = reader["region_name"].ToString();
                    city_name = reader["city_name"].ToString();
                    ip_from = reader["ip_from"].ToString();
                    ip_to = reader["ip_to"].ToString();
                    if (country_name == "-")   //将-转换为LAN
                    {
                        region_name = "LAN";
                        city_name = "LAN";
                        country_name = "LAN";
                    }
                    reader.Close();
                }
                thisConnection.Close();
            }
                catch (Exception e)   
                {
                    country_code = "null";
                    country_name = "null";
                    city_name = "null";
                    region_name = "null";
                    ip_from = "null";
                    ip_to = "null";
                    thisConnection.Close();
                }
            }
        }

        public double Dot2LongIP(string DottedIP)   //将ip转换为double
        {
            int i;
            string[] arrDec;
            double num = 0;
            if (DottedIP == "")
            {
                return 0;
            }
            else
            {
                arrDec = DottedIP.Split('.');
                for (i = arrDec.Length - 1; i >= 0; i--)
                {
                    num += ((int.Parse(arrDec[i]) % 256) * Math.Pow(256, (3 - i)));
                }
                return num;
            }
        }
    }
}
   




