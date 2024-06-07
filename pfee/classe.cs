using System;

namespace WhiteList
{
    public class ClasseDrives
    {
        public string drivesClass { get; set; }
        public string guid { get; set; }
        public string id { get; set; }
        // Autres propriétés de l'instance

        public ClasseDrives(string guid ,string drivesClass , string id)
{
    this.guid = guid;
    this.drivesClass = drivesClass;
    
    this.id = id;
}
    }
    public class ClasseD
    {
        public string Name { get; set; }
        public string GUID { get; set; }
        public string Chemin { get; set; }

        public bool isAutorised {get; set;}
        

        public ClasseD(string name, string guid, string chemin , bool auto)
        {
            Name = name;
            GUID = guid;
            Chemin = chemin;
            isAutorised = auto ;
        }
        
                
    }
    
}