using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.UX
{
    class AdditionalUX
    {
		/// <summary>
		/// AnimateCursor method is used to create a simple cursor animation in the console.
		/// </summary>
		/// <param name="animationDuration"></param>
		/// <param name="dotCount"></param>
		public void AnimateCursor(
            int animationDuration = 3000, 
            int dotCount = 3,
            string message = ""
            )
        {
            if (!string.IsNullOrEmpty(message))
			{
				Console.Write(message + " ");
			}

			Console.Write("> ");

			int dotPosition = 0;
            var startTime = DateTime.Now;

            while((DateTime.Now - startTime).TotalMilliseconds < animationDuration)
            {
                // clear previous dots
                for (int i  = 0; i < dotCount; i++)
                {
                    Console.Write(" ");
                }

                // return cursor to starting position
                Console.Write(new string('\b', dotCount + 1));

                // Write current number of dots
                for (int i = 0; i < dotPosition + 1; i++)
				{
					Console.Write(".");
				}

                // Increment dot position and wrap around if needed
                dotPosition = (dotPosition + 1) % dotCount;

                // Wait before next animation frame
                Thread.Sleep(300);

                // return cursor to start, to prepare for next iteration
                Console.Write(new string('\b', dotCount + 1));
			}

            // Clear the animation when done
            Console.Write(new string(' ', dotCount + 2));
			Console.Write(new string('\b', dotCount + 2));
		}

    }
}
