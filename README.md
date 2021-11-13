# ToxicPlants
 
| Branch | Status |   
| :--- | :--- |
| develop  | ![Actions build and test](https://github.com/animundo/ToxicPlants/actions/workflows/dotnetdevelop.yml/badge.svg?branch=develop) |
| main  | ![Actions build and test](https://github.com/animundo/ToxicPlants/actions/workflows/dotnetmain.yml/badge.svg?branch=main) |


## Usage

This is an Azure function with http based trigger. The program is so small, that making a complete webserver is an overkill. It has one endpoint post / plantcheck

It is used as an restapi. you can sent images of plants and an animal name and the api returns if the plant is recognised (in a persentage) and if it could be toxic for that animal.   
   
The idea is that you can sent pictures with a mobile phone to this api to get results.
