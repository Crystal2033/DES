using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.ThreadingWork
{
    public sealed class FileDataLoader
    {
		public static int TextBlockSize = 2000;
		private byte[] _textBlock;
		private int _factTextBlockSize;
		public int FactTextBlockSize { get; set; }
			
		public byte[] TextBlock
		{
			get;
			set;
		} = new byte[TextBlockSize];

		private int currentPosInFile;
		private BinaryReader _fileReadFrom;
		private BinaryWriter _fileWriteTo;

        public FileDataLoader(string fileReadFrom, string fileWriteTo)
		{
			
            _fileReadFrom = new BinaryReader(File.OpenRead(fileReadFrom), Encoding.UTF8);
            _fileWriteTo = new BinaryWriter(File.OpenRead(fileWriteTo), Encoding.UTF8); ;
        }

		public void reloadTextBlockAndOutputInFile() 
		{
			if(currentPosInFile != 0)
			{
				insertTextBlockInFile();
            }

            _factTextBlockSize = _fileReadFrom.Read(_textBlock, currentPosInFile, TextBlockSize);
			currentPosInFile += _factTextBlockSize;
        }

        private void insertTextBlockInFile()
        {
			if(_factTextBlockSize != 0)
			{
                _fileWriteTo.Write(_textBlock, 0, _factTextBlockSize);
            } 
        }
    }
}
