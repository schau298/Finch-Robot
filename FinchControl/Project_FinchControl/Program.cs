using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;
using System.Linq;

namespace Project_FinchControl
{
    // **************************************************
    //
    // Title: Finch Control
    // Description: The Talent Show, Data Recorder, and Light Alarm System
    // Application Type: Console
    // Author: Schaub, Dylan
    // Dated Created: 2/12/2021
    // Last Modified: 2/28/2021
    //
    // **************************************************
    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>

        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>

        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;
        }



        /// <summary>
        /// ****************
        /// *   Main Menu  *          
        /// ****************
        /// </summary>

        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");
                //
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");

                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "d":
                        LightAlarmDisplayMenuScreen(finchRobot);
                        break;

                    case "e":

                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        


        #region DATA RECORDER


        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            
            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperature = null;

            Console.CursorVisible = true;

            bool quitDataRecorder = false;
            string menuChoice;

            do 

            {
                DisplayScreenHeader("Data Recorder Menu");

                
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine($"\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        temperature = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayGetData(temperature);
                        break;
                    
                    case "q":
                        quitDataRecorder = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitDataRecorder);
        }

        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            Console.CursorVisible = true;
            DisplayScreenHeader("Number of data points");

            Console.Write("\n\tEnter the number of data points: ");
            string userResponse = Console.ReadLine();

            int.TryParse(userResponse, out int numberOfDataPoints); 

            DisplayContinuePrompt();
            return numberOfDataPoints;
        }

        static double DataRecorderDisplayGetDataPointFrequency()
        {
            Console.CursorVisible = true;
            DisplayScreenHeader("Frequency of data points in seconds:");

            double.TryParse(Console.ReadLine(), out double dataPointFrequency); 

            DisplayContinuePrompt();
            return dataPointFrequency;
        }



        static void DataRecorderDisplayGetData(double[] temperature)
        {
            DisplayScreenHeader("Show Data");

            DataRecorderDisplayTable(temperature);

            DisplayContinuePrompt();
        }

        static void DataRecorderDisplayTable(double[] temperature)
        {
            Console.WriteLine(
                "Recording #".PadLeft(15) +
                "Temp".PadLeft(15)
                );
            Console.WriteLine(
                "***********".PadLeft(15) +
                "***********".PadLeft(15)
                );

            //
            // display Table
            //

            for (int index = 0; index < temperature.Length; index++)
            {
                Console.WriteLine(
                (index + 1).ToString().PadLeft(15) +
                temperature[index].ToString("n2").PadLeft(15)
                );
            }
        }


        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency,
            Finch finchRobot)
        {
            
            double[] temperature = new double[numberOfDataPoints];

            DisplayScreenHeader("Get Data");
            Console.WriteLine($"\tNumber of data points: {numberOfDataPoints}");
            Console.WriteLine($"\tData point frequency: {dataPointFrequency}");
            Console.WriteLine();
            Console.WriteLine("\tThe finch robot is ready to begin recording the temperature data.");
            DisplayContinuePrompt();

            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperature[index] = finchRobot.getTemperature();
   
                Console.WriteLine($"\tReading {index + 1}: {temperature[index].ToString("n2")}");
                int waitInSeconds = (int)(dataPointFrequency * 1000);
                finchRobot.wait(waitInSeconds);
            }
            DisplayContinuePrompt();
            DisplayScreenHeader("Get Data");

            Console.WriteLine();
            Console.WriteLine("\t Table of Temperature");
            Console.WriteLine();
            DataRecorderDisplayTable(temperature);


            DisplayContinuePrompt();

            return temperature;
        }

        #endregion

        #region ALARM SYSTEM
        ///
        /// <summary>
        /// *********************
        /// *  Light Alarm Menu *
        /// *********************
        /// </summary>
        /// <param name="finchRobot"></param>


        static void LightAlarmDisplayMenuScreen(Finch finchRobot)
        {

            Console.CursorVisible = true;

            bool quitAlarm = false;
            string menuChoice;

            string sensorsToMonitor ="";
            string rangeType = "";
            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;


            do

            {
                DisplayScreenHeader("Light Alarm Menu");


                Console.WriteLine("\ta) Set Sensors to Monitor ");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Minimum/Maximum Threshold Value");
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine($"\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        break;

                    case "b":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;

                    case "c":
                        minMaxThresholdValue = LightAlarmSetMinMaxThresholdValue(rangeType,finchRobot);
                        break;

                    case "d":
                        timeToMonitor = LightAlarmSetTimeToMonitor();
                        break;
                    case "e":
                        LightAlarmSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;

                    case "q":
                        quitAlarm = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitAlarm);
        }

        private static void LightAlarmSetAlarm(
            Finch finchRobot,
            string sensorsToMonitor,
            string rangeType,
            int minMaxThresholdValue,
            int timeToMonitor)
        {
            int secondsElapsed = 0;
            bool thresholdExceeded = false;
            int currentLightSensorValue = 0;
            

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"\t Sensors to monitor {sensorsToMonitor}");
            Console.WriteLine("\t Range Type: {0}", rangeType);
            Console.WriteLine("\t Min/Max threshold value: " + minMaxThresholdValue);
            Console.WriteLine($"\t Time to monitor: {timeToMonitor}");
            Console.WriteLine();

            Console.WriteLine("\t Press any key to begin monitoring:");
            Console.ReadKey();
            Console.WriteLine();

            while ((secondsElapsed < timeToMonitor) && !thresholdExceeded)
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        currentLightSensorValue = finchRobot.getLeftLightSensor();
                        break;

                    case "right":
                        currentLightSensorValue = finchRobot.getRightLightSensor();
                        break;

                    case "both":
                        currentLightSensorValue = (finchRobot.getRightLightSensor() + finchRobot.getLeftLightSensor()) / 2;
                        break;
                }

                switch (rangeType)
                {
                    case "minimum":
                        if (currentLightSensorValue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;

                    case "maximum":
                        if (currentLightSensorValue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }

                        break;
                }
                finchRobot.wait(1000);
                secondsElapsed++;
                Console.WriteLine("\tCurrent Value: {0} ", currentLightSensorValue);
            }

            if (thresholdExceeded)
            {
                Console.WriteLine($"\t The {rangeType} threshold value of {minMaxThresholdValue} was exceeded by the current light sensor value of {currentLightSensorValue}.");
                finchRobot.setLED(255, 0, 255);
                finchRobot.noteOn(100);
                finchRobot.wait(1000);
                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
            }
            else
            {
                Console.WriteLine($"\t The {rangeType} threshold value of {minMaxThresholdValue} was not exceeded.");
                finchRobot.setLED(0, 255, 0);
                finchRobot.wait(1000);
                finchRobot.setLED(0, 0, 0);
            }


            DisplayMenuPrompt("Light Alarm");
        }



        //
        static int LightAlarmSetMinMaxThresholdValue(string rangeType, Finch finchRobot)
        {
            int minMaxThresholdValue;

            bool minMaxThresholdValidValue;
            
            DisplayScreenHeader("Minimum/Maximum Threshold Value");

            Console.WriteLine($"\t Left light sensor ambient value: {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"\t Right light sensor ambient value: {finchRobot.getRightLightSensor()}");
            Console.WriteLine();

            Console.Write("\t Enter the {rangeType} light sensor value:");

            minMaxThresholdValidValue = int.TryParse(Console.ReadLine(), out minMaxThresholdValue);
             
            if (!minMaxThresholdValidValue)
            {
                DisplayScreenHeader("Please enter integer: ");
                DisplayContinuePrompt();
                return LightAlarmSetTimeToMonitor();
            }
            else
            {
                return minMaxThresholdValue;
            }

        }




        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string sensorsToMonitor;
            List<string> validSensor = new List<string>() { "left", "right", "both"};

            DisplayScreenHeader("Sensors To Monitor");

            Console.Write("\t Sensors to monitor [left, right, both]:");
            sensorsToMonitor = Console.ReadLine().ToLower();

            if (validSensor.Contains(sensorsToMonitor))
            {
                return sensorsToMonitor;
            }
            else
            {
                DisplayScreenHeader("Please input  [left, right, or both] for sensors to monitor");
                DisplayContinuePrompt();
                return LightAlarmDisplaySetSensorsToMonitor();

            }

        }

        static string LightAlarmDisplaySetRangeType()
        {
            string rangeType;
            string[] validRange = new string[] { "minimum", "maximum" };

            DisplayScreenHeader("Range Type");

            Console.Write("\t Range Type [minimum, maximum]:");
            rangeType = Console.ReadLine().ToLower();


            if (validRange.Contains(rangeType))
            {
                return rangeType;
            }
            else
            {
                DisplayScreenHeader("Please input minimum or maximum:");
                DisplayContinuePrompt();
                return LightAlarmDisplaySetRangeType();
            }
        }


        static int LightAlarmSetTimeToMonitor()
        {
            int timeToMonitor;
            bool timeToMonitorValid;
            DisplayScreenHeader("Time To Monitor");

            Console.Write("\t Time to Monitor: ");
            timeToMonitorValid = int.TryParse(Console.ReadLine(), out timeToMonitor);

             
            if (!timeToMonitorValid)
            {
                DisplayScreenHeader("Please enter an integer");
                DisplayContinuePrompt();
                return LightAlarmSetTimeToMonitor();
            }
            else
            {
                return timeToMonitor;
            }
        }



        #endregion



        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                Console.WriteLine($"Welcome to the selection screen!");
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance");
                Console.WriteLine("\tc) Mixing It Up");
                Console.WriteLine("\td) ");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        TalentShowDisplayLightAndSound(finchRobot);
                        break;

                    case "b":
                        TalentShowDisplayDance(finchRobot);
                        break;

                    case "c":
                        TalentShowDisplayMixingItUp(finchRobot);
                        break;

                    case "d":

                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void TalentShowDisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will now show off its glowing talent!");
            DisplayContinuePrompt();


            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 10);
            }

            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOn(0);

            DisplayMenuPrompt("Talent Show Menu");
        }

        #endregion

        #region Dance

        ///
        ///**************
        ///*    Dance   *
        ///**************
        ///
        ///


        static void TalentShowDisplayDance(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Dance");

            Console.WriteLine("\t Get ready!");

            DisplayContinuePrompt();
            for (int t = 1; t < 4; t++)
            {
                finchRobot.setMotors(80, 100);
                System.Threading.Thread.Sleep (1000);
                finchRobot.setMotors(0, 100);
                System.Threading.Thread.Sleep(1500);
                finchRobot.setMotors(0, 0);
            }

            DisplayMenuPrompt("Talent Show Menu");
        }

        #endregion Dance



        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Mixing it Up                    *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void TalentShowDisplayMixingItUp(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Mixing it up");

            Console.WriteLine("\tThe Finch robot will now dance and flash lights");
            DisplayContinuePrompt();

            for (int t = 0; t < 4; t++)
            {
                finchRobot.setMotors(80, 100);
                for (int lightSoundLevel = 0; lightSoundLevel < 100; lightSoundLevel++)
                {
                    finchRobot.setLED(0, lightSoundLevel, lightSoundLevel);
                    finchRobot.noteOn(lightSoundLevel * 50);
                }
                finchRobot.setMotors(0, 100);
                for (int lightSoundLevel = 100; lightSoundLevel > 0; lightSoundLevel--)
                {
                    finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                    finchRobot.noteOn(lightSoundLevel * 50);
                }
                finchRobot.setMotors(0, 0);
            }
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOn(0);

            DisplayMenuPrompt("Talent Show Menu");
        }





        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            // TODO test connection and provide user feedback - text, lights, sounds

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion

    }
}
