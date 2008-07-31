﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    class HsvColour
    {
        internal static Color HsvToColor(int hue, int saturation, int value)
        {
            double h;
            double s;
            double v;

            double r = 0;
            double g = 0;
            double b = 0;

            h = ((double)hue / 255 * 360) % 360;
            s = (double)saturation / 255;
            v = (double)value / 255;

            if (s == 0)
            {
                r = v;
                g = v;
                b = v;
            }
            else
            {
                double p;
                double q;
                double t;

                double fractionalSector;
                int sectorNumber;
                double sectorPos;

                sectorPos = h / 60;
                sectorNumber = (int)(Math.Floor(sectorPos));

                fractionalSector = sectorPos - sectorNumber;

                p = v * (1 - s);
                q = v * (1 - (s * fractionalSector));
                t = v * (1 - (s * (1 - fractionalSector)));

                switch (sectorNumber)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }

            return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }
    }
}
