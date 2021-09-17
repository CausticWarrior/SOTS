﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Prim;

namespace SOTS.Items.Pyramid.Aten
{
	class AtenPrimTrail : PrimTrail
	{
		public AtenPrimTrail(Projectile projectile)
		{
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
			Color = new Color(255, 230, 138);
			Width = 20;
			Cap = 20;
			Pixellated = false;
		}

		public override void SetDefaults() => AlphaValue = 1f;

		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			if (PointCount <= 6) return;
			float widthVar;
			for (int i = 0; i < Points.Count; i++)
			{
				if (i == 0)
				{
					widthVar = MathHelper.Lerp(0, Width, i / (float)Points.Count);
					Color colorvar = Color.Lerp(Color, new Color(255, 160, 40), (i / (float)Points.Count));
					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;
					AddVertex(Points[i], colorvar * AlphaValue, Vector2.Zero);
					AddVertex(secondUp, colorvar * AlphaValue, Vector2.Zero);
					AddVertex(secondDown, colorvar * AlphaValue, Vector2.Zero);
				}
				else
				{
					if (i != Points.Count - 1)
					{
						widthVar = MathHelper.Lerp(0, Width, i / (float)Points.Count);
						Color colorvar = Color.Lerp(Color, new Color(255, 160, 40), ((float)i / (float)Points.Count));
						Vector2 normal = CurveNormal(Points, i);
						Vector2 normalAhead = CurveNormal(Points, i + 1);
						Vector2 firstUp = Points[i] - normal * widthVar;
						Vector2 firstDown = Points[i] + normal * widthVar;
						Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
						Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

						AddVertex(firstDown, colorvar * AlphaValue, new Vector2((i / ((float)Points.Count)), 1));
						AddVertex(firstUp, colorvar * AlphaValue, new Vector2((i / ((float)Points.Count)), 0));
						AddVertex(secondDown, colorvar * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 1));

						AddVertex(secondUp, colorvar * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 0));
						AddVertex(secondDown, colorvar * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 1));
						AddVertex(firstUp, colorvar * AlphaValue, new Vector2((i / ((float)Points.Count)), 0));
					}
				}
			}
		}

		public override void SetShaders()
		{
			Effect effect = SOTS.AtenTrail;
			effect.Parameters["TrailTexture"].SetValue(ModContent.GetInstance<SOTS>().GetTexture("TrailTextures/Trail_3"));
			effect.Parameters["ColorOne"].SetValue(Color.ToVector4());
			effect.Parameters["ColorTwo"].SetValue(Color.ToVector4());
			PrepareShader(effect, "MainPS", Counter / 24f);
		}

		public override void OnUpdate()
		{
			if (!(Entity is Projectile proj))
				return;

			Counter++;
			PointCount = Points.Count() * 6;

			if (Cap < PointCount / 6)
				Points.RemoveAt(0);

			if ((!Entity.active && Entity != null) || Destroyed)
				OnDestroy();

			else
				Points.Add(Entity.Center);
		}
		public override void OnDestroy()
		{
			Destroyed = true;
			Width *= 0.8f;
			Width += ((float)Math.Sin(Counter * 2) * 0.3f);
			if (Width < 0.05f)
				Dispose();
		}
	}
}