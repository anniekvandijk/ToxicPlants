# ToxicPlants
Buildstatus   
![Actions build and test](https://github.com/animundo/ToxicPlants/actions/workflows/dotnet.yml/badge.svg?branch=master)   
Code coverage   
[![codecov](https://codecov.io/gh/Animundo/ToxicPlants/branch/master/graph/badge.svg?token=LKYYZ9E0MH)](https://codecov.io/gh/Animundo/ToxicPlants)


## Usage

This is an Azure function with http based trigger. The program is so small, that making a complete webserver is an overkill. It has one endpoint post / plantcheck

It is used as an restapi. you can sent images of plants and an animal name and the api returns if the plant is recognised (in a persentage) and if it could be toxic for that animal.   
   
The idea is that you can sent pictures with a mobile phone to this api to get results.