﻿using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using TgcViewer;
using Dx3D = Microsoft.DirectX.Direct3D;

namespace AlumnoEjemplos.AtTheEndOfTheDay.ThePerfectElement
{
    public class Wall : Item
    {
        #region Constants
        private const Single _DefaultMaxScale = 10;
        private const Single _DefaultMinScale = 2.5f;
        #endregion Constants

        #region Constructors
        private readonly IndependentParticlePart _Dust;
        public Wall()
        {
            var mesh = Game.Current.NewMesh("Wall");
            _MeshTextured = new MeshStaticPart(Game.Current.NewMesh("WallTextured"));
            Add(_Mesh = new MeshStaticPart(mesh));
            Add(new ObbCollider(mesh));
            Add(_Dust = new IndependentParticlePart()
            {
                Translation = new Vector3(0, 0, -4),
                Sound = Game.Current.GetSound("WallStrike.wav", EffectVolume),
                Animation = new AnimatedQuad()
                {
                    Texture = Game.Current.GetParticle("Dust.png"),
                    FrameSize = new Size(512, 102),
                    FirstFrame = 0,
                    CurrentFrame = 0,
                    FrameRate = 15,
                    TotalFrames = 5,
                },
                Size = new Vector2(25, 15),
            }); 
            MaxScale = _DefaultMaxScale;
            MinScale = _DefaultMinScale;
        }
        #endregion Constructors

        #region Properties
        public Single MinScale { get; set; }
        public Single MaxScale { get; set; }
        private MeshStaticPart _Mesh;
        public Color Color
        {
            get { return _Mesh.Color; }
            set { _Mesh.Color = value; }
        }
        private MeshStaticPart _MeshTextured;
        private String _Texture;
        public String Texture
        {
            get { return _Texture; }
            set
            {
                if (_Texture == value) return;
                _Texture = value;
                if (String.IsNullOrWhiteSpace(value))
                    _SwapMeshes(false);
                else
                {
                    try
                    {
                        _MeshTextured.Texture = Game.Current.GetMaterial(value);
                        _SwapMeshes(true);
                    }
                    catch { _SwapMeshes(false); }
                }
            }
        }
        private Boolean _IsTextured = false;
        private void _SwapMeshes(Boolean isTextured)
        {
            if (_IsTextured == isTextured) return;
            if (_IsTextured = isTextured)
            {
                Add(_MeshTextured);
                Remove(_Mesh);
            }
            else
            {
                Add(_Mesh);
                Remove(_MeshTextured);
            }
        }
        #endregion Properties

        #region ResetMethods
        public override void LoadValues()
        {
            base.LoadValues();
            _Dust.Stop();
        }
        #endregion ResetMethods

        #region ItemMethods
        public override void Build(Single deltaTime)
        {
            var input = GuiController.Instance.D3dInput;
            var stepS = deltaTime * BuildScalingSpeed;
            if (input.keyDown(Key.E))
                Scale = Scale.AdvanceX(stepS, MaxScale);
            else if (input.keyDown(Key.Q))
                Scale = Scale.AdvanceX(stepS, MinScale);
            var stepR = deltaTime * BuildRotationSpeed;
            if (input.keyDown(Key.D))
                Rotation = Rotation.AddZ(-stepR);
            else if (input.keyDown(Key.A))
                Rotation = Rotation.AddZ(stepR);
        }
        public override void Animate(Single deltaTime)
        {
            _Dust.Update(deltaTime);
        }
        protected override void OnContact(ItemContactState contactState, Single deltaTime)
        {
            base.OnContact(contactState, deltaTime);
            _Dust.Start(contactState.Point, contactState.Approach, contactState.Normal);
        }
        #endregion ItemMethods
    }
}
