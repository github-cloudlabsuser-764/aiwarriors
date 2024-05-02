$(document).ready(function () {
       // Handle category change to load sub-categories
        $('#category').change(function () {
            var categoryId = $(this).val();
            
            if (categoryId) {               
                $.getJSON('/ShelfOptimization/GetSubCategories', { category: categoryId }, function (data) {
                    var subCategoryDropdown = $('#subcategory');
                    subCategoryDropdown.empty();
                    subCategoryDropdown.append('<option value="">Select Sub-category</option>');
                    $.each(data, function (index, item) {
                        subCategoryDropdown.append('<option value="' + item + '">' + item + '</option>');
                    });
                    subCategoryDropdown.prop('disabled', false);
                });
            } else {
                $('#subcategory').empty().prop('disabled', true);
                $('#productlist').empty().prop('disabled', true);
            }
        });

    // Handle sub-category change to load products
    $('#subcategory').change(function () {
                var subCategoryId = $(this).val();

    if (subCategoryId) {
        $.getJSON('/ShelfOptimization/GetProducts', { subCategory: subCategoryId }, function (data) {
            var productListBox = $('#productlist');
            productListBox.empty();
            $.each(data, function (index, item) {
                productListBox.append('<option value="' + item.productId + '">' + item.productName + '</option>');
            });
            productListBox.prop('disabled', false);
        });
                } else {
        $('#productlist').empty().prop('disabled', true);
                }
            });

    $("#addProducts").click(function () {
        // Get a reference to the select elements  
        var productList = $("#productlist");
        var selectedProductList = $("#selectedprdlist");

        // Loop through each option in the product list, and append it to the selected product list  
        productList.find("option:selected").each(function (index, item) {
            selectedProductList.append($(this).clone());
        });
    });  


    // Handle Optimization products
    $('#btnRecommendation').click(function () {
        var selectedProductIds = $("#selectedprdlist").val();
        console.log(selectedProductIds);
        var rows = $('#grdRow').val();
        var columns = $('#grdCol').val();
        var season = $("#season").val();

        if (selectedProductIds && selectedProductIds.length >= 0)
        {

            var serializedIds = selectedProductIds.map(encodeURIComponent).join("&selectedProductIds=");

            var queryString = `?selectedProductIds=${serializedIds}&season=${encodeURIComponent(season)}&rows=${rows}&columns=${columns}`;

            //var st = "{  \"Row 1\": {    \"Box 1\": [\"Shampoo\", \"Conditioner\", \"Hair Color\"],    \"Box 2\": [\"Hair Conditioner\", \"Hair Dye\", \"Hair Brush\"],    \"Box 3\": [\"Hair Comb\"]  },  \"Row 2\": {    \"Box 1\": [\"Hair Gel\", \"Hair Curlers\", \"Kids Shampoo\"],    \"Box 2\": [\"Hair Mousse\", \"Hair Serum\"],    \"Box 3\": [\"Hair Bands\", \"Hair Rollers\", \"Shaving Cream\"]  }}";

            
            $.getJSON('/ShelfOptimization/GetRecommendation' + queryString, function (data) {
                console.log(data.message.value);
                var jsonData = JSON.parse(data.message.value);
                var messageData = jsonData.choices[0].message.content;
                var backtickJson;
                if (messageData.startsWith("{")) {
                    backtickJson = JSON.parse(messageData);
                }
                else {
                    let regex = /`([^`]+)`/g;
                    let matches = messageData.match(regex);
                    let backtickString = matches[0].replace(/`/g, '');
                    backtickJson = JSON.parse(backtickString);
                }
                
                console.log(backtickJson);  
                createGrid(rows, columns, backtickJson);
            })
                .fail(function () {
                    alert('Error');
                });

            //$.getJSON('/ShelfOptimization/GetRecommendation', { selectedProductIds: selectedProductIds, season, rows, columns },
            //    function (data) {

            //        alert('1');
            //});

        }
        else {
                    $('#result').text('Please select at least one product.');
              }
    });

    //Transaction
    $('#btnTransaction').click(function () {

        var selectedChannel = $("#channel").val();
        if (selectedChannel && selectedChannel.length >= 0) {

            var queryString = `?selectedChannel=${selectedChannel}`;


            $.getJSON('/TransactionData/GetSeleactedTransactions' + queryString, function (data) {
                console.log(data.message.value);
                var jsonData = JSON.parse(data.message.value);
                var messageData = jsonData.choices[0].message.content;
                var backtickJson;
                if (messageData.startsWith("{")) {
                    backtickJson = JSON.parse(messageData);
                }
                else {
                    let regex = /`([^`]+)`/g;
                    let matches = messageData.match(regex);
                    let backtickString = matches[0].replace(/`/g, '');
                    backtickJson = JSON.parse(backtickString);
                }

                console.log(backtickJson);
                ('#div-result').append(createTable(data));
            })
                .fail(function () {
                    alert('Error');
                });
        }
    });
      
});

function createGrid(rows, columns, data) {
    var gridContainer = $("#grid-container"); // Get the grid container
    gridContainer.empty(); // Clear any existing content

    // Set the grid structure based on rows and columns
    gridContainer.css("grid-template-columns", `repeat(${columns}, 1fr)`);
    gridContainer.css("grid-template-rows", `repeat(${rows}, 1fr)`);

    // Loop through the data to populate the grid
    var rowIndex = 0;
    for (var row in data) {
        if (data.hasOwnProperty(row) && rowIndex < rows) {
            var boxIndex = 0;
            for (var box in data[row]) {
                if (data[row].hasOwnProperty(box) && boxIndex < columns) {
                    // Create a new grid cell for each box
                    var gridCell = $("<div></div>").addClass("grid-cell");

                    // Populate the grid cell with the content from the JSON data
                    //gridCell.html(`<strong>${row} - ${box}:</strong><br>`);
                    gridCell.html(data[row][box].join(", "));

                    // Set the position in the grid
                    gridCell.css("grid-column-start", boxIndex + 1);
                    gridCell.css("grid-row-start", rowIndex + 1);

                    // Add the cell to the grid container
                    gridContainer.append(gridCell);

                    boxIndex++; // Move to the next box
                }
            }
            rowIndex++; // Move to the next row
        }
    }
}

function createTable(data) {
    let tableHtml = '<table border="1">';
    tableHtml += '<tr><th>Category</th><th>Product</th><th>Countries</th></tr>';

    for (const category in data) {
        const products = data[category];
        for (const product in products) {
            const regions = products[product];
            tableHtml += `<tr><td>${category}</td><td>${product}</td><td>${regions}</td></tr>`;
        }
    }

    tableHtml += '</table>';

    return tableHtml;
}
// Example usage
//$(document).ready(function () {
//    // Sample JSON data for a 3x3 grid
//    var gridData = {
//        "Row 1": {
//            "Box 1": ["Shampoo", "Conditioner", "Hair Color"],
//            "Box 2": ["Hair Dryer", "Hair Comb", "Hair Clippers"],
//            "Box 3": ["Hair Dye", "Hair Gel", "Hair Spray"]
//        },
//        "Row 2": {
//            "Box 1": ["Shaving Cream", "Shaving Razor", "Shaving Blades"],
//            "Box 2": ["Aftershave", "Shaving Gel", "Aftershave Balm"],
//            "Box 3": ["Beard Oil", "Beard Balm", "Beard Comb"]
//        },
//        "Row 3": {
//            "Box 1": ["Hair Mousse", "Hair Serum", "Hair Rollers"],
//            "Box 2": ["Kids Shampoo", "Hair Bands", "Hair Curlers"],
//            "Box 3": ["Beard Trimmer", "Shaving Brush", "Electric Razors"]
//        }
//    };

//    // Create a 3x3 grid with the given data
//    createGrid(3, 3, gridData);
//});

//Sample JSONDATA for transactions

//    {
//        "Electronics": {
//            "Laptop": {
//                "North America": 2,
//                "India": 1
//            },
//            "Smartphone": {
//                "North America": 1,
//                "India": 1
//            }
//        },
//        "ELectricals": {
//            "Wires": {
//                "North America": 2,
//                "India": 1
//            },
//            "Cables": {
//                "North America": 1,
//                "India": 1
//            }
//        }
//    }

