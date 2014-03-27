/*
 * Copyright (c) 2011 BitSits Games
 *  
 * Shubhajit Saha    http://bitsits.blogspot.com/
 * Maya Agarwal      http://bitsitsgames.blogspot.com/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    class MenuEntry
    {
        #region Fields

        GameScreen screen;

        /// <summary>
        /// The text rendered for this entry.
        /// </summary>
        string text;
        Texture2D texture;
        Vector2 position;

        float size = 30;

        public object UserData;

        public Vector2 footerPosition = new Vector2(-1);
        public string footers = string.Empty;

        /// <summary>
        /// Tracks a fading selection effect on the entry.
        /// </summary>
        /// <remarks>
        /// The entries transition out of the selection effect when they are deselected.
        /// </remarks>
        float selectionFade;

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the text of this menu entry.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                if (texture == null)
                {
                    Vector2 textSize = screen.ScreenManager.GameContent.symbolFont.MeasureString(text);
                    textSize *= size / screen.ScreenManager.GameContent.symbolFontSize;
                    return new Rectangle((int)position.X, (int)position.Y, (int)textSize.X, (int)textSize.Y);
                }
                else
                    return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }

        #endregion

        #region Events


        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;


        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new menu entry with the specified text and location.
        /// </summary>
        public MenuEntry(string text, Vector2 position, GameScreen screen)
        {
            this.screen = screen;
            this.text = text;
            this.position = position;
        }

        /// <summary>
        /// Constructs a new menu entry with the specified texture and location.
        /// </summary>
        public MenuEntry(Texture2D texture, Vector2 position, GameScreen screen)
            : this(string.Empty, position, screen)
        {
            this.texture = texture;
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(bool isSelected, GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }


        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(bool isSelected, GameTime gameTime)
        {
            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.WhiteSmoke : Color.Gray;

            if (texture != null) color = isSelected ? Color.White : Color.Gainsboro;

            // Modify the alpha to fade text out during transitions.
            color = new Color(color, screen.TransitionAlpha);

            // Draw text, centered on the middle of each line.
            GameContent gameContent = screen.ScreenManager.GameContent;
            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;

            float scale = 0.03f * selectionFade;

            if (texture == null)
                spriteBatch.DrawString(gameContent.symbolFont, text, position, color, 0,
                    Vector2.Zero, size / gameContent.symbolFontSize + scale, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(texture, position, null, color, 0, Vector2.Zero, 1 + scale, SpriteEffects.None, 1);

            if (footerPosition == new Vector2(-1))
                footerPosition = position + new Vector2(0, BoundingRectangle.Height + 5);

            if (isSelected)
                spriteBatch.DrawString(gameContent.symbolFont, footers, footerPosition, color, 0,
                        Vector2.Zero, 15f / gameContent.symbolFontSize, SpriteEffects.None, 1);

            if (screen is LevelMenuScreen && footers == string.Empty)
                spriteBatch.Draw(gameContent.cross, position, null, color, 0, Vector2.Zero,
                    1 + scale, SpriteEffects.None, 1);
        }


        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight()
        {
            return screen.ScreenManager.GameContent.debugFont.LineSpacing;
        }


        #endregion
    }
}
