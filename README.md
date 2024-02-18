# This Project is to response to Emenu_Backend_Challange 
This project has 8 main endpoint 
- CollectionPhoto
- Collection
- ProductPhoto
- Product
- Size
- Color
- Material
- Photo
<br> All this endpoints have GetList,Save,Delete
<br>Save will inserte to database if the object id = 0  if the object id is mentioned then the save api will do the update
## The CollectionPhoto endpoint is the main endpoint where it will return list of the products and its variants 
This api has 5 parameters 
- filter: string filtering of the result
- sortingCol: to determaine of which colume the sorting will be.this parameter accepte one of the following value "nameEn,nameAr,description,colorName,sizeName,materialName" otherwise the sorting colume will be on Id
- isDescending:bool value to determine sorting Ascending or Descending
- skip: int value of the skipped item in the list
- take:int value on how many items will be return
<br>call example for CollectionPhoto getList endpoint 
```
https://localhost:7165/api/CollectionPhoto/GetList?isDescending=false&skip=0&take=10&filter=%20&sortingCol=%20
```
response example
```
{
  "result": [
    {
      "collectionId": 1,
      "collection": {
        "product": {
          "id": 1,
          "nameAr": "بلوزة",
          "nameEn": "T-shirt",
          "description": "very good T-shirtjeans",
          "mainPhotoUrl": "B",
          "size": "Medium",
          "color": "Red",
          "material": "Silk",
          "photoes": [
            {
              "id": 4,
              "url": "A"
            },
            {
              "id": 5,
              "url": "B"
            }
          ]
        }
      }
    },
    {
      "collectionId": 2,
      "collection": {
        "product": {
          "id": 1,
          "nameAr": "بلوزة",
          "nameEn": "T-shirt",
          "description": "very good T-shirtjeans",
          "mainPhotoUrl": "B",
          "size": "Medium",
          "color": "Red",
          "material": "Coton",
          "photoes": [
            {
              "id": 2,
              "url": "d"
            }
          ]
        }
      }
    }
  ],
  "enumResult": 200,
  "validationResults": null,
  "errorMessages": ""
}
```
## get started
clone this repositrey to visual studio,then go to appSetting.json and update key connectionString value
```
"ConnectionStrings": {
    "Key": "Please Update Here"
  },
```
please run Emenu_Backend_Challange Project.the execution will open swagger page please follow swagger instraction to call all the end point
