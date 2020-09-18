# StockExchangeAdvisor

The created program recommends the purchase/sale of shares on the basis of quotations for the WIG20 exchange. The idea is as follows:

1. Downloading current quotations from the Internet resources
2. Calculation of selected/all technical indicators (including EMA, SMA, ROC, Candle Formation...)
3. Consolidation of results and determination of the recommended action regarding the purchase/sale of shares in the company
4. On the basis of the decision, calculation of the broker's commission, tax due and possible currency conversion fee
5. The result of the program is a series of files in .CSV format, which store data together with quotes and obtained decisions for each quotation of each analyzed company

What is more, the program allows you to choose one of 3 methods of distributed processing. 
Distributed processing was used to speed up the process of time-consuming calculation of technical indicators for a huge amount of data. 
Additionally, in order to speed up the work of the program, it has been adapted for parallel processing in places
where it affects performance (apart from segments processed in a distributed way).

Available technologies of distributed processing:

* RabbitMQ
* WebServices (based on Azure CentOS virtual machine and ASP.NET Core Web API)
* Akka.net (Actor-Model)

Benchmark - 3630 companies and 5 technical indicators calculated (i7 4790K, tested on localhost(focusing on the direct delay of certain technology)):

| First Header  | Second Header |
| ------------- | ------------- |
| Content Cell  | Content Cell  |
| Content Cell  | Content Cell  |

| Technology   | Total time (minutes)   | Time per one company (milliseconds)  |
| ---- ------- |:----------------------:| ------------------------------------:|
| RabbitMQ     | 23.21                  | 385                                  |
| WebServices  | 45.37                  | 753                                  |
| ActorModel   | 10.40                  | 176                                  |

The design code was designed for easy expansion, therefore a number of design patterns were used:

* Adapter
* Builder
* Chain of Responsibility
* Decorator
* Facade
* Observer
* Prototype
* State
* Strategy

The use was also made of Reflection and Serialization.
