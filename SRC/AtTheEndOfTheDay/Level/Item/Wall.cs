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
    public class Wall : Item
    {
        #region Constructors
        private const String _MeshName = "Wall";
        private const Single _MinScaleXFactor = 0.5f;
        private const Single _MaxScaleXFactor = 2;

        #region TexturedConstructors
        public Wall(Game game, String material)
            : this(game, Vector3.Empty, Vector3.Empty, Vector3.Empty, material) { }
        public Wall(Game game, Vector3 position, String material)
            : this(game, position, Vector3.Empty, Vector3.Empty, material) { }
        public Wall(Game game, Vector3 position, Vector3 scale, String material)
            : this(game, position, scale, Vector3.Empty, material) { }
        public Wall(Game game, Vector3 position, Vector3 scale, Vector3 rotation, String material)
            : this(position, scale, rotation, game.NewMesh(_MeshName, material)) { }
        #endregion TexturedConstructors

        #region ColorConstructors
        public Wall(Game game, Color color)
            : this(game, Vector3.Empty, Vector3.Empty, Vector3.Empty, color) { }
        public Wall(Game game, Vector3 position, Color color)
            : this(game, position, Vector3.Empty, Vector3.Empty, color) { }
        public Wall(Game game, Vector3 position, Vector3 scale, Color color)
            : this(game, position, scale, Vector3.Empty, color) { }
        public Wall(Game game, Vector3 position, Vector3 scale, Vector3 rotation, Color color)
            : this(position, scale, rotation, game.NewMesh(_MeshName, color)) { }
        #endregion ColorConstructors

        #region DefaultConstructors
        public Wall(Game game)
            : this(game, Vector3.Empty, Vector3.Empty, Vector3.Empty) { }
        public Wall(Game game, Vector3 position)
            : this(game, position, Vector3.Empty, Vector3.Empty) { }
        public Wall(Game game, Vector3 position, Vector3 scale, Vector3 rotation)
            : this(position, scale, rotation, game.GetMesh(_MeshName)) { }
        private Wall(Vector3 position, Vector3 scale, Vector3 rotation, TgcMesh mesh)
        {
            Add(new MeshStaticPart(mesh));
            Add(new ObbCollider(mesh));
            Scale = scale;
            _MinSacleX = scale.X * _MinScaleXFactor;
            _MaxSacleX = scale.X * _MaxScaleXFactor;
            Rotation = rotation;
            Position = position;
        }
        #endregion DefaultConstructors
        #endregion Constructors

        #region ItemMethods
        private readonly Single _MinSacleX;
        private readonly Single _MaxSacleX;
        public override void Build(Single deltaTime)
        {
            var input = GuiController.Instance.D3dInput;
            var stepS = deltaTime * BuildScalingSpeed;
            if (input.keyDown(Key.E))
                Scale = Scale.AdvanceX(stepS, _MaxSacleX);
            else if (input.keyDown(Key.Q))
                Scale = Scale.AdvanceX(stepS, _MinSacleX);
            var stepR = deltaTime * BuildRotationSpeed;
            if (input.keyDown(Key.D))
                Rotation = Rotation.AddZ(-stepR);
            else if (input.keyDown(Key.A))
                Rotation = Rotation.AddZ(stepR);
        }
        #endregion ItemMethods
    }
}
