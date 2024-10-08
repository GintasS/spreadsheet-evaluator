<div align="center">
  <h1>Spreadsheet evalutor</h1>
  <br />
  <br />
</div>

<details open="open">
<summary>Table of Contents</summary>

- [About](#about)
  - [Built With](#built-with)
- [Features](#features)
- [Example data](#example-data)
- [Usage](#usage)
- [System Design](#system-design)
- [Screenshots](#screenshots)
- [License](#license)

</details>

---

## About

<table>
<tr>
<td>

This project is a command-line interface (CLI) application developed using C# and .NET, designed to parse numerical spreadsheets, perform calculations, and write the results to a file. 
The spreadsheets are in a JSON format, and they can contain numbers, text and formulas. Spreadsheets can have invalid cells, with the exception that the JSON structure is correct.

Formulas can be like "A1 + A2", thus we are using recursion with numerous validation techniques to validate data and perform calculations.

It was initially developed to call an API, get spreadsheets from JSON, evaluate the spreadsheet, perform calculations and send results to another API endpoint.
Now, it uses CLI to ask for input and ouput files.

</td>
</tr>
</table>

### Built With

- C#
- .NET 8
- Console
- Moq
- Fluent Assertions
- and other NUGET packages

## Features

<ul>
  <li>Features Domain-Driven design (DDD) architecture.</li>
  <li>The project has exit codes, console logs to inform the user about the program's state.</li>
  <li>Exception handling, displaying errors to the user.</li>
  <li>Using 3rd party libraries to ease our work: to help with json parsing, DI, unit testing and more.</li>
  <li>Application settings. You can set hard-coded values inside appsettings.json.</li>
  <li>We also have different class models (DTOs) for different HTTP GET and POST requests.</li>
  <li>For the testing part, we use xUnit, mocking, fluent assertions and more.</li>
  <li>We use AAA (Act-Arrange-Assert) structure for our tests.</li>
</ul>

### Example data

You can find both input and expected output files in the ```data``` folder.


### Usage


### System Design

You can find System Design notes in the Product Search.pdf file.


## Screenshots

<img src="docs/screenshots/screenshot1.JPG" width="800" height="400"><br>
The main Swagger view.

<img src="docs/screenshots/screenshot2.JPG" width="800" height="300"><br>
The main endpoint for recommending products and filtering price.

<img src="docs/screenshots/screenshot6.JPG" width="800" height="300"><br>
The response from the main endpoint that both recommends products and can filter by price.

<img src="docs/screenshots/screenshot3.JPG" width="800" height="300"><br>
The second endpoint for recommending products by product description only.

<img src="docs/screenshots/screenshot4.JPG" width="800" height="300"><br>
The third endpoint for filtering products by the price only.

<img src="docs/screenshots/screenshot5.JPG" width="800" height="300"><br>
The response from the third endpoint that filters products by the price only.

<img src="docs/screenshots/screenshot7.JPG" width="800" height="300"><br>
The example of how a prompt to ChatGPT can look like.

[![Video demo](https://img.youtube.com/vi/jadqKtgYj2U/0.jpg)](https://www.youtube.com/watch?v=jadqKtgYj2U)

## License

This project is licensed under the **MIT license**. Feel free to edit and distribute this template as you like.

See [LICENSE](LICENSE) for more information.
