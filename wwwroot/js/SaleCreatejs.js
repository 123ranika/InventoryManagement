

// Initialize arrays and variables
let items = [];

function addItem() {
    // Get values from input fields
    const description = document.querySelector('input[name="Description"]').value;
    const quantity = parseFloat(document.querySelector('input[name="Quantity"]').value);
    const unitPrice = parseFloat(document.querySelector('input[name="UnitPrice"]').value);
   
    // Validate inputs
    if (!description || !quantity || !unitPrice) {
        Swal.fire({
            position: "top-end",
            icon: "warning",
            title: "Please Fill Description, price and quantity",
            timer: 1500,
            showConfirmButton: false,
            customClass: {
                popup: 'small-alert'
            }
        });
        return;
    }

    // Calculate total price for this item
    const subtotal = quantity * unitPrice;
   
    const totalPrice = subtotal;

    // Create new item object
    const newItem = {
        description,
        quantity,
        unitPrice,
        subtotal,
        totalPrice
    };

    // Add to items array
    items.push(newItem);
  

    // Update the table
    updateTable();

    // Clear input fields
    clearInputFields();

    // Update totals
    updateOrderSummary();
}

//function to calculate discout in both ammount and %
function calculateDiscount(subtotal, discountValue) {
    if (discountType === '%') {
        return (subtotal * discountValue) / 100;
    } else {
        return Math.min(discountValue, subtotal);
    }
}

function updateTable() {
    const tableBody = document.getElementById('items-container');
    tableBody.innerHTML = ''; // Clear existing rows

    items.forEach((item, index) => {
        const row = `
                    <tr>
                        <td class="product-name">${item.description}</td>
                        <td class="product-qty">${item.quantity}</td>
                        <td class="product-price">${item.unitPrice}</td>                      
                        <td class="product-total">${item.totalPrice.toFixed(2)}</td>
                        <td>
                            <button type="button" class="btn btn-danger btn-sm" onclick="removeItem(${index})">
                                <i class="bi bi-trash"></i> Remove
                            </button>
                        </td>
                    </tr>
                `;
        tableBody.insertAdjacentHTML('beforeend', row);
    });
}

function removeItem(index) {


    items.splice(index, 1);


    updateTable();
    updateOrderSummary();
}

function clearInputFields() {
    document.querySelector('input[name="Description"]').value = '';
    document.querySelector('input[name="Quantity"]').value = '1';
    document.querySelector('input[name="UnitPrice"]').value = '';
    document.querySelector('input[name="TotalPrice"]').value = '';
}

function updateOrderSummary() {
    // Calculate subtotal
    const subtotal = items.reduce((sum, item) => sum + item.subtotal, 0);

    // Get manual discount
    const manualDiscount = parseFloat(document.getElementById('ManualDiscount').value) || 0;

    // Calculate grand total
    const grandTotal = subtotal - manualDiscount;

    // Get amount paid
    const payAmount = parseFloat(document.getElementById('Pay').value) || 0;
    console.log(payAmount);

    // Calculate due amount
    const dueAmount = grandTotal - payAmount;

    // Update display fields

    document.getElementById('subtotal').value = subtotal.toFixed(2);
    document.getElementById('grandTotal').value = Math.max(0, grandTotal).toFixed(2);
    document.getElementById('Due').value = Math.max(0, dueAmount).toFixed(2);
}

// Add event listeners for real-time total calculation
document.querySelector('input[name="Quantity"]').addEventListener('input', calculateLineTotal);
document.querySelector('input[name="UnitPrice"]').addEventListener('input', calculateLineTotal);

// Add event listener for manual discount
document.getElementById('ManualDiscount').addEventListener('input', function (e) {
    // Sanitize input to allow only numbers and decimal point
    let value = e.target.value.replace(/[^\d.]/g, '');

    // Ensure only one decimal point
    const parts = value.split('.');
    if (parts.length > 2) value = parts[0] + '.' + parts.slice(1).join('');

    // Limit to 2 decimal places
    if (parts.length > 1) value = parts[0] + '.' + parts[1].slice(0, 2);

    e.target.value = value;
    updateOrderSummary();
});

// Add event listener for Due field with same behavior as ManualDiscount
document.getElementById('Due').addEventListener('input', function (e) {
    // Sanitize input to allow only numbers and decimal point
    let value = e.target.value.replace(/[^\d.]/g, '');

    // Ensure only one decimal point
    const parts = value.split('.');
    if (parts.length > 2) value = parts[0] + '.' + parts.slice(1).join('');

    // Limit to 2 decimal places
    if (parts.length > 1) value = parts[0] + '.' + parts[1].slice(0, 2);

    //Sending to function to calculation
    e.target.value = value;
    updateOrderSummary();
});

document.getElementById('Pay').addEventListener('input', function (e) {
    updateOrderSummary();
});

function calculateLineTotal() {
    const quantity = parseFloat(document.querySelector('input[name="Quantity"]').value) || 0;
    const unitPrice = parseFloat(document.querySelector('input[name="UnitPrice"]').value) || 0;
  

    const total = quantity * unitPrice;
    //const discountAmount = calculateDiscount(subtotal, discountValue);
   
    document.querySelector('input[name="TotalPrice"]').value = total.toFixed(2);
}




$(document).ready(function () {
    // Function to collect the invoice data dynamically when the button is clicked
    function getInvoiceData(isPrint = false) {
        var selectedPaymentMethod = $('input[name="paymentMethod"]:checked').val();

        return {
            CustomerId: $('#CustomerId').val(),
            Name: $('#Name').val(),
            Phone: $('#Phone').val(),
            Address: $('#Address').val(),
            GrandTotal: $('#grandTotal').val(),
            InvoiceID: $('#Invoice_ID').val(),
            Total_Discount: $('#totalDiscount').val(),
            Due: $('#Due').val(),
            Pay: $('#Pay').val(),
            Date: $('#Date').val(),
            Discount: $('#ManualDiscount').val(),
            PaymentMethod: selectedPaymentMethod,
            IsPrint: isPrint,
            InvoiceItems: items.map(item => ({
                TotalPrice: item.totalPrice,
                Description: item.description,
                Quantity: item.quantity,       
                Price: item.unitPrice
            }))
        };
    }

    // Function to validate form
    function validateForm() {
        var isValid = true;
        var errorMessages = [];

        // Check payment method
        if (!$('input[name="paymentMethod"]:checked').val()) {
            errorMessages.push("Please select a payment method");
            isValid = false;
        }

        // Check phone number
        var phone = $('#Phone').val();
        if (!phone || phone.trim() === '') {
            errorMessages.push("Phone number is required");
            isValid = false;
        }

        if (!items || items.length === 0) {
            errorMessages.push("Invoice items cannot be empty");
            isValid = false;
        } else {
            // Check if any of the invoice item fields are missing or invalid
            items.forEach((item, index) => {
                if (!item.description || item.description.trim() === '') {
                    errorMessages.push(`Description is required for item ${index + 1}`);
                    isValid = false;
                }
                if (!item.quantity || item.quantity <= 0) {
                    errorMessages.push(`Quantity must be greater than 0 for item ${index + 1}`);
                    isValid = false;
                }
                if (!item.unitPrice || item.unitPrice <= 0) {
                    errorMessages.push(`Price must be greater than 0 for item ${index + 1}`);
                    isValid = false;
                }
            });
        }

        if (!isValid) {
            Swal.fire({
                position: "top-end",
                icon: "warning",
                title: "Required Fields Missing",
                html: errorMessages.join('<br>'),
                timer: 2000,
                showConfirmButton: false,
                customClass: {
                    popup: 'small-alert'
                }
            });
            return false;
        }
        return true;
    }

});





var clearForm = function () {
    // Reset form
    $('#invoiceForm')[0].reset();
    // Reset payment method radio buttons
    $('input[name="paymentMethod"]').prop('checked', false);

    //refreshing invoice id
    var newInvoiceId = new Date().toISOString().replace(/[-T:.Z]/g, "").slice(0, 14);
    $("#Invoice_ID").val(newInvoiceId);

    //resetting order list
    items = [];
    updateTable();

    //resetting order summary
    document.getElementById('subtotal').value = "0.00";
    document.getElementById('totalDiscount').value = "0.00";
    document.getElementById('ManualDiscount').value = "";
    document.getElementById('grandTotal').value = "0.00";
    document.getElementById('Due').value = "";
    document.getElementById('Pay').value = "0.00";
    updateOrderSummary();
}





$(document).ready(function () {
    // Debounce function to limit API calls
    function debounce(func, delay) {
        let timeoutId;
        return function (...args) {
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => func.apply(this, args), delay);
        };
    }

    // Function to handle suggestion population
    function populateSuggestion(item) {
        $('#CustomerId').val(item.id || '0');
        $('#Name').val(item.name || '');
        $('#Phone').val(item.phone || '');
        $('#Address').val(item.address || '');
        $('#suggestions').hide();
    }
     function populateProductSuggestion(item) {
         debugger;
         $('#Description').val(item.productName || '');
         $('#UnitPrice').val(item.unitPrice || '');

         $('#productsuggestions').hide();
    }

    // Fetch phone suggestions
    const fetchPhoneSuggestions = debounce(function (phoneNumber) {
        if (phoneNumber.length < 3) {
            $('#suggestions').hide();
            return;
        }

        $.ajax({
            url: '/Invoice/SearchClientByPhone',
            type: 'GET',
            data: { phone: phoneNumber },
            success: function (data) {
                const suggestionsDiv = $('#suggestions');
                suggestionsDiv.empty().hide();
                
                if (data.success && data.references && data.references.length) {
                    data.references.forEach(function (item) {
                        const suggestionItem = $(`
                                    <div class="suggestion-item"
                                         style="padding: 5px; cursor: pointer;"
                                         data-id="${item.clientID}"
                                         data-name="${item.clientName}"
                                         data-phone="${item.phone}"
                                         data-address="${item.address}">
                                         ${item.clientName} - ${item.phone}
                                    </div>
                                `);

                        suggestionItem.on('click', function () {
                            populateSuggestion({
                                id: $(this).data('id'),
                                name: $(this).data('name'),
                                phone: $(this).data('phone'),
                                address: $(this).data('address')
                            });
                        });

                        suggestionsDiv.append(suggestionItem);
                    });
                    suggestionsDiv.show();
                } else {
                    suggestionsDiv.append(`<div style="padding: 5px;">No suggestions found</div>`);
                    suggestionsDiv.show();
                }
            },
            error: function () {
                console.error('Error fetching phone suggestions.');
                $('#suggestions').hide();
            }
        });
    }, 300); // 300ms delay to reduce unnecessary API calls

    // Fetch Product suggestions
    const fetchProductSuggestions = debounce(function (description) {
        if (description.length < 3) {
            $('#productsuggestions').hide();
            return;
        }

        $.ajax({
            url: '/Invoice/SearchProductByName',
            type: 'GET',
            data: { description: description },
            success: function (data) {
                const suggestionsDiv = $('#productsuggestions');
                suggestionsDiv.empty().hide();


                debugger;
                if (data.success && data.references && data.references.length) {
                    data.references.forEach(function (item) {
 
                        const suggestionItem = $(`
                                    <div class="suggestion-item"
                                         style="padding: 5px; cursor: pointer;"
                                         data-anikaName="${item.productName}"
                                         data-anikaPrice="${item.price}">
                                         ${item.productName}
                                    </div>
                                `);

                        suggestionItem.on('click', function () {
                            populateProductSuggestion({
                               
                                productName: $(this).data('anikaName'),
                                unitPrice: $(this).data('anikaPrice')
                            });
                        });

                        suggestionsDiv.append(suggestionItem);
                    });
                    suggestionsDiv.show();
                } else {
                    suggestionsDiv.append(`<div style="padding: 5px;">No suggestions found</div>`);
                    suggestionsDiv.show();
                }
            },
            error: function () {
                console.error('Error fetching phone suggestions.');
                $('#suggestions').hide();
            }
        });
    }, 300); // 300ms delay to reduce unnecessary API calls

    // Phone input event handler
    $('#Phone').on('input', function () {
        const phoneNumber = $(this).val();
        $('#CustomerId').val('0'); // Reset customer ID on input
        fetchPhoneSuggestions(phoneNumber);
    });
    $('#Description').on('input', function () {
        const description = $(this).val();
        fetchProductSuggestions(description);
    });

    // Close suggestions when clicking outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('#Phone, #suggestions').length) {
            $('#suggestions').hide();
        }
    });
    // Close suggestions when clicking outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('#Description, #productsuggestions').length) {
            $('#productsuggestions').hide();
        }
    });

    //save data
    $('#SaveButton').click(function (e) {
        
        e.preventDefault();

        const productList = [];
        $('#items-container tr').each(function () {
            const row = $(this);
            productList.push({
                productName: row.find('.product-name').text(),
                quantity: parseInt(row.find('.product-qty').text()),
                price: parseFloat(row.find('.product-price').text()),
                total: parseFloat(row.find('.product-total').text())
            });
        });

        const clients = {
            customerName: $('#Name').val(),           
            address: $('#Address').val(),
            phone: $('#Phone').val(),
        }
        console.log(clients);


        const invoice = {    
            date: $('#Date').val(),
            products: productList,
            subtotal: parseFloat($('#subtotal').val()) || 0,
            discount: parseFloat($('#ManualDiscount').val()) || 0,
            grandTotal: parseFloat($('#grandTotal').val()) || 0,
            pay: parseFloat($('#Pay').val()) || 0,
            due: parseFloat($('#Due').val()) || 0,
            paymentType: $('input[name="paymentMethod"]:checked').val(),
            slip: parseInt($('#Invoice_ID').val()) || 0,
            client: clients

        };

        console.log(invoice);
        
        console.log(productList);

        debugger;

        //$.ajax({
        //    type: "POST",
        //    url: "/Invoice/OrderSummarySubmit",
        //    contentType: "application/json",
        //    data: JSON.stringify(invoice),
        //    success: function (response) {
        //        alert("Invoice submitted successfully!");
        //        // Optionally redirect or clear form here
        //    },
        //    error: function (xhr) {
        //        alert("Submission failed: " + xhr.responseText);
        //    }
        //});
    });
});

