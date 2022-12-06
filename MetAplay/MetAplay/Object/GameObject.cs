using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetAplay
{
    public class GameObject
    {
        public GameObjectType ObjectType { get; protected set; } = GameObjectType.None;
        public int Id
        {
            get { return Info.Id; }
            set { Info.Id = value; }
        }

        public GameRoom JoinedRoom { get; set; }

        public ObjectInfo Info { get; set; } = new ObjectInfo();


        public GameObject()
        {
        }

        public virtual void Update()
        {

        }
    }
}
