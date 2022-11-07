using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MazeConsoleGame
{
    class UserInterface
    {
        private readonly RestClient client = new RestClient("https://coding-challanges.herokuapp.com/api/mazes/");
        private string mazeId;
        private MazeLocation currentLocation;

        public void run()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string file = System.IO.Path.Combine(currentDirectory, @"..\..\..\..\login.json");
            //string filePath = Path.GetFullPath(file);
            //StreamReader reader = new StreamReader(filePath);
            string jsonString = File.ReadAllText(file);
            MazeUser user = JsonSerializer.Deserialize<MazeUser>(jsonString);

            client.Authenticator = new HttpBasicAuthenticator(user.Username, user.Password);
            RestRequest newMazeRequest = new RestRequest("/?level=" + user.Difficulty, Method.Post);
            RestResponse newMazeResponse = client.Execute(newMazeRequest);
            mazeId = newMazeResponse.Content;

            

            bool exit = false;
            Console.WriteLine("Welcome to the Cretan maze!");
            Console.WriteLine("Enter one of the commands below to navigate the maze.");
            Console.WriteLine("Find all three coins and excape through the exit to win.");
            Console.WriteLine("You have one hour before your torch goes out and you are stuck in the maze forever.");
            Console.WriteLine("Good luck!");
            showCommands();
            while (!exit)
            {
                currentLocation = getLocation();
                Console.WriteLine(currentLocation);
                exit = getPlayerInput();
            }

        }
        public MazeLocation getLocation()
        {
            RestRequest locationRequest = new RestRequest(mazeId + "/steps", Method.Get);
            RestResponse<MazeLocation> locationResponse = client.Execute<MazeLocation>(locationRequest);
            return locationResponse.Data;
        }
  
        public bool getPlayerInput()
        {
            Console.Write("Enter your next move: ");
            string userInput = Console.ReadLine().ToLower();
            if(userInput == "u" || userInput == "up" || userInput == "n" || userInput == "north")
            {
                movePlayer("north");
            }
            else if(userInput == "d" || userInput == "down" || userInput == "s" || userInput == "south")
            {
                movePlayer("south");
            }
            else if(userInput == "l" || userInput == "left" || userInput == "w" || userInput == "west")
            {
                movePlayer("west");
            }
            else if(userInput == "r" || userInput == "right" || userInput == "e" || userInput == "east")
            {
                movePlayer("east");
            }
            else if(userInput == "x" || userInput == "exit" || userInput == "q" || userInput == "quit")
            {
                giveUp();
                return true;
            }
            else if(userInput == "o" || userInput == "options" || userInput== "h" || userInput == "help")
            {
                showCommands();
            }
            else if(userInput == "p" || userInput == "pickup" || userInput == "g" || userInput == "grab")
            {
                pickupCoin();
            }
            else if(userInput == "t" || userInput == "time" || userInput == "i" || userInput == "info")
            {
                showInfo();
            }
            else
            {
                Console.WriteLine("Invalid input!");
                Console.WriteLine("Please enter a valid command");
                Console.WriteLine("Enter \"help\" for help");
            }
            return false;
        }

        public void movePlayer(string direction)
        {
            if(direction == "north")
            {
                RestRequest moveRequest = new RestRequest(mazeId + "/steps?direction=NORTH", Method.Post);
                RestResponse moveResponse = client.Execute(moveRequest);
                Console.WriteLine(moveResponse.Content);
            }
            else if(direction == "east")
            {
                RestRequest moveRequest = new RestRequest(mazeId + "/steps?direction=EAST", Method.Post);
                RestResponse moveResponse = client.Execute(moveRequest);
                Console.WriteLine(moveResponse.Content);
            }
            else if(direction == "south")
            {
                RestRequest moveRequest = new RestRequest(mazeId + "/steps?direction=SOUTH", Method.Post);
                RestResponse moveResponse = client.Execute(moveRequest);
                Console.WriteLine(moveResponse.Content);
            }
            else if(direction == "west")
            {
                RestRequest moveRequest = new RestRequest(mazeId + "/steps?direction=WEST", Method.Post);
                RestResponse moveResponse = client.Execute(moveRequest);
                Console.WriteLine(moveResponse.Content);
            }
            else
            {
                Console.WriteLine("Somehow an invalid direction was accepted.");
                Console.WriteLine("This should be an unreachable case and probably means the entire program is broken.");
                Console.WriteLine("I'd suggest either restarting the program, or if applicable, fix whatever you broke in the source code to allow this to happen.");
            }
        }

        public void pickupCoin()
        {
            RestRequest pickupRequest = new RestRequest(mazeId + "/coins", Method.Post);
            RestResponse pickupResponse = client.Execute(pickupRequest);
        }
   
        public void showCommands()
        {
            Console.WriteLine("\nTo move through the maze you will need to pick a direction.");
            Console.WriteLine("Remember that bumping into a wall will cause you to drop all your coins.");
            Console.WriteLine("To move north enter: n, north, u, or up");
            Console.WriteLine("To move east enter: e, east, r, or right");
            Console.WriteLine("To move south enter: s, south, d, or down");
            Console.WriteLine("To move west enter: w, west, l, or left");
            Console.WriteLine("If you are standing on a coin you can pick it up by entering: p, pickup, g, or grab");
            Console.WriteLine("To get maze information such as time remaining, coins collected, or time started: t, time, i, info");
            Console.WriteLine("If you need to see this list again you can enter: o, options, h, or help");
            Console.WriteLine("Finally, if you want to give up and exit the maze you can enter: x, exit, q, or quit\n");
        }

        public void showInfo()
        {
            RestRequest infoRequest = new RestRequest(mazeId, Method.Get);
            RestResponse<MazeData> infoResponse = client.Execute<MazeData>(infoRequest);
            Console.WriteLine(infoResponse.Data);
        }

        public void giveUp()
        {
            RestRequest giveUpRequest = new RestRequest(mazeId, Method.Delete);
            client.Execute(giveUpRequest);
        }
    }
}
