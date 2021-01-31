using System;
using System.Linq;
using System.Text;

namespace AzureFileUploader
{
    class Progress : IProgress<long>
    {
        private readonly long fileLength;
        private readonly int pace;
        private readonly char progressBarBeginChar = '(';
        private readonly char progressBarPaceNotReachedChar = '|';
        private readonly char progressBarPaceReachedChar = '-';
        private readonly char progressBarEndChar = ')';

        private StringBuilder progressBar;
        private int lastPercentage;
        private int currentPace;

        public Progress(long fileLength, int pace = 5)
        {
            if (pace < 1 || pace > 100)
                throw new ArgumentOutOfRangeException($"{nameof(pace)} must be between 1 and 100");

            this.pace = pace;
            this.fileLength = fileLength;

            var progressBarDrawing = new string(Enumerable.Range(0, (100 / pace)).Select(i => progressBarPaceNotReachedChar).ToArray());
            progressBar = new StringBuilder(progressBarBeginChar + progressBarDrawing + progressBarEndChar);
        }

        public void Report(long value)
        {
            if (value == 0)
            {
                currentPace = 0;
                Console.WriteLine(progressBar);
            }

            var percentage = (int)((value / (double)fileLength) * 100);
            if (percentage > 0 && percentage != lastPercentage && percentage % pace == 0)
            {
                currentPace++;
                PrintProgress(currentPace);
            }
            lastPercentage = percentage;
        }


        private void PrintProgress(int currentPace)
        {
            if (currentPace != 0)
                ClearCurrentConsoleLine();

            var progressBarNow = progressBar.Replace(progressBarPaceNotReachedChar, progressBarPaceReachedChar, 0, currentPace + 1).ToString();
            Console.WriteLine(progressBarNow);
        }

        public static void ClearCurrentConsoleLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}