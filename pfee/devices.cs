using System;


namespace WhiteList
{
    public class Instance
    {
        public string InstanceId { get; set; }
        // Autres propriétés de l'instance

        public Instance(string instanceId)
        {
            InstanceId = instanceId;
        }
    }
    public class Device
    {
        public string IdDevice { get; set; }
        public string NameD { get; set; }
        public string Manufacture { get; set; }
        public string GuidD { get; set; }
        public string infs { get; set; }
        public List<Instance> Instances { get; } = new List<Instance>();


        public Device(string idDevice, string nameD, string manufacture, string guidD , string inf)
        {
            IdDevice = idDevice;
            NameD = nameD;
            Manufacture = manufacture;
            GuidD = guidD;
            infs = inf;
        }
        public void AddInstance(string instanceId)
        {
            Instances.Add(new Instance(instanceId));
        }
    }
}
