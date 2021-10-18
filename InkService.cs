using Microsoft.Extensions.Hosting;

using Ink.Runtime;
using Ink;
using Microsoft.Extensions.FileProviders;

namespace net6test
{
    public class InkService
    {
        public InkService()
        {
        }

        public Story LoadStory(string file) => CompileFile(file);

        private async Task InkMain(Story story)
        {
            while(true)
            {
                while (story.canContinue) {
                    var str = story.Continue();
                    System.Console.WriteLine(str);
                }
                if(story.currentChoices.Count > 0 )
                {
                    for (int i = 0; i < story.currentChoices.Count; ++i) {
                        Choice choice = story.currentChoices [i];
                        System.Console.WriteLine("Choice " + (i + 1) + ". " + choice.text);
                    }
                    int choiceNo;
                    string input;
                    do {
                        input = Console.ReadLine();
                    } while(!int.TryParse(input, out choiceNo));
                    story.ChooseChoiceIndex(choiceNo-1);
                } else {
                    break;
                }
            }
        }


        private Ink.Runtime.Story CompileFile(string filename)
        {
            var compiler = CreateCompiler(filename);

            var story = compiler.Compile();
            story.onError += OnError;

            return story;
        }

        private void OnError(string message, ErrorType type)
        {
            throw new Exception(message);
        }

        private Compiler CreateCompiler(string filename)
        {
            var inkSource = File.ReadAllText(filename);

            return new Compiler(inkSource, new Compiler.Options
            {   
                sourceFilename = filename,
                errorHandler = OnError 
            });
        }
    }
}