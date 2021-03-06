﻿using System.Collections.Generic;
using System.IO;
using LiteDB;

namespace DediServerCron.Data
{
    public class DataStorage
    {
        
        public static bool CheckFile(string file, string path)
        {
            if (!File.Exists(path + file))
            {
                
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void UpdateOrCreateServer(string server)
        {
            string theServer = "";
            using (var db = new LiteDatabase(@"Data\servers.db"))
            {
                var servers = db.GetCollection<Servers>("servers");
            

            var results = servers.FindOne(x => x.ServerName.Contains(server));
                if (results.ServerName == server)
                {
                    theServer = results.ServerName;
                }
                else
                {
                    AddServerToDb(server);
                }

                
            }
    }

        public static void AddServerToDb(string server)
        {
            var db = new LiteDatabase(@"Data\servers.db");
            var servers = db.GetCollection<Servers>("servers");
            var newServer = new Servers
            {
                ServerName = server,
                BatchFileLocation = "test"
            };
            servers.Insert(newServer);
        }
        public static IEnumerable<Servers> GetDbInfo()
        {
            using (var db = new LiteDatabase(@"Data\servers.db"))
            {

                var servers = db.GetCollection<Servers>("servers");

                var result = servers.FindAll();
                
                return result;
            }
        }

        public static Servers GetDbInfobyId(int id)
        {
            using (var db = new LiteDatabase(@"Data\servers.db"))
            {

                var servers = db.GetCollection<Servers>("servers");

                var result = servers.FindById(id);

                return result;
            }
        }

        public static LiteCollection<Servers> GetDb()
        {
            using (var db = new LiteDatabase(@"Data\servers.db"))
            {

                var servers = db.GetCollection<Servers>("servers");

                

                return servers;
            }
        }
    }
}