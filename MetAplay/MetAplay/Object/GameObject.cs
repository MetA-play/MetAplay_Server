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

        public TransformInfo Transform
        {
            get => Info.Transform;
            set => Info.Transform = value;
        }

        public GameObject()
        {
            Info = new ObjectInfo();
            Info.Transform = new TransformInfo();
            Info.Transform.Pos = new Vector();
            Info.Transform.Rot = new Vector();
            Info.Transform.Scale  = new Vector();
        }

        public virtual void Update()
        {

        }
        
        public void SetPosition(float x, float y, float z)
        {
            Info.Transform.Pos.X = x;
            Info.Transform.Pos.Y = y;
            Info.Transform.Pos.Z = z;
        }
    }
}
