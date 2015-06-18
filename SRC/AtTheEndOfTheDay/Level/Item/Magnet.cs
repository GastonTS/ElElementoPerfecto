﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TgcViewer;
using TgcViewer.Example;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Input;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using Dx3D = Microsoft.DirectX.Direct3D;

namespace AlumnoEjemplos.AtTheEndOfTheDay.ThePerfectElement
{
    public class Magnet : Item
    {
        #region Constants
        private const Single _ForceFactor = 100000f;
        #endregion Constants

        #region Constructors
        private readonly IndependentParticlePart _Spark;
        public Magnet()
        {
            var mesh = Game.Current.GetMesh("Magnet");
            Add(_Spark = new IndependentParticlePart()
            {
                Translation = new Vector3(0, 0, -4),
                Sound = Game.Current.GetSound("iman.wav", EffectVolume),
                Animation = new AnimatedQuad()
                {
                    Texture = Game.Current.GetParticle("SparksFinal.png"),
                    FrameSize = new Size(256, 256),
                    FirstFrame = 0,
                    CurrentFrame = 0,
                    FrameRate = 30,
                    TotalFrames = 10,
                },
                Size = new Vector2(25, 15),
            }); 
            Add(new MeshStaticPart(mesh));
            Add(_Obb = new ObbTranslatedCollider(mesh));
        }
        #endregion Constructors

        #region Properties
        private Single _Force;
        private Single _ForceReal;
        public Single Force
        {
            get { return _Force; }
            set
            {
                _Force = value;
                _ForceReal = value * _ForceFactor;
            }
        }
        #endregion Properties
        
        #region ItemMethods
        public override void Build(Single deltaTime)
        {
            var input = GuiController.Instance.D3dInput;
            var stepR = deltaTime * BuildRotationSpeed;
            if (input.keyDown(Key.D))
                Rotation = Rotation.AddZ(-stepR);
            else if (input.keyDown(Key.A))
                Rotation = Rotation.AddZ(stepR);
        }

        public override void LoadValues()
        {
            _Spark.Stop();
            base.LoadValues();
        }

        public override void Animate(float deltaTime)
        {
            _Spark.Update(deltaTime);
            base.Animate(deltaTime);
        }

        private readonly ObbTranslatedCollider _Obb;
        public override void Act(Interactive interactive, Single deltaTime)
        {
            var n = interactive.Position - Position;
            var d2 = n.LengthSq();
            n.Normalize();
            if (Vector3.Dot(n, _Obb.Orientation[1]) > .7f)
                interactive.Momentum -= n * (_ForceReal / d2);
        }
        #endregion ItemMethods

        public override void ReceiveCollision(Vector3 point, float approachVel, Vector3 normal)
        {
            _Spark.Start(point, approachVel, normal);
        }
    }
}
