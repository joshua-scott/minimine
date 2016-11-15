using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace minimine
{
    public abstract class BoardGame
    {
        protected UIElement[] places;

        abstract public void Init();

        public virtual void NewGame()
        {

        }

        public virtual void Play(object sender)
        {

        }
    }
}
