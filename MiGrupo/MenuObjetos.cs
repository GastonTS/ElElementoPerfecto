﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using TgcViewer;
using TgcViewer.Example;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class MenuObjetos 
    {
        private TgcSprite menu;
        private TgcSprite[] objetos;
        Size screenSize = GuiController.Instance.Panel3d.Size;
        TgcTexture texturaMenu = TgcTexture.createTexture(EjemploAlumno.alumnoTextureFolder() + "menu3.jpg");

        public MenuObjetos()
        {               
            //Creacion de Sprites
            menu = new TgcSprite();
            objetos = new TgcSprite[8];
            for(var i = 0; i < 8; i++)
                objetos[i] = new TgcSprite();

            //Asginar Texturas
            menu.Texture = texturaMenu;
            foreach (var sprite in objetos)
                sprite.Texture = TgcTexture.createTexture(EjemploAlumno.alumnoTextureFolder() + "menu2.jpg");      
        }

        public void renderMenu()
        {
            menu.render();
            for (var i = 0; i < 8; i++)
                objetos[i].render();
        }

        public void disposeMenu()
        {
            menu.dispose();
            for (var i = 0; i < 8; i++)
                objetos[i].dispose();
        }

        public void actualizarModifiers()
        {//Esta pensado para una resolucion con aspect ratio 16:9
            //Actualizacion de Menu
            float ladoIzqMenu = screenSize.Width * (1 - 0.25f);
            float anchoMenu = screenSize.Width - ladoIzqMenu;
            menu.Position = new Vector2(ladoIzqMenu, 0);
            menu.SrcRect = new Rectangle(0, 0, (int)Math.Round(anchoMenu), screenSize.Height);
            
            //Actualizacion de Objetos de Menu
            float ladoObjeto = anchoMenu *0.5f;
            for (var i = 0; i < 4; i++)
            {
                objetos[i].Position = new Vector2(ladoIzqMenu, 0 + ladoObjeto * i);
                objetos[i].Scaling = new Vector2(ladoObjeto / objetos[0].Texture.Width, ladoObjeto / objetos[0].Texture.Height);
            }
            for (var i = 4; i < 8; i++)
            {
                objetos[i].Position = new Vector2(ladoIzqMenu + ladoObjeto, 0 + ladoObjeto * (i - 4));
                objetos[i].Scaling = new Vector2(ladoObjeto / objetos[0].Texture.Width, ladoObjeto / objetos[0].Texture.Height);
            }
            
        }

    }
}
