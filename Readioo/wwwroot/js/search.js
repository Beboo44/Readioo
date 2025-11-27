// Search functionality
$(document).ready(function () {
    console.log('Search script loaded'); // Debug log

    var searchTimeout;
    var $searchInput = $('#bookSearch');
    var $searchResults = $('#searchResults');

    // Input event handler
    $searchInput.on('input', function () {
        console.log('Input detected:', $(this).val()); // Debug log

        var query = $(this).val().trim();

        // Clear previous timeout
        clearTimeout(searchTimeout);

        // Hide results if query is too short
        if (query.length < 2) {
            $searchResults.hide().html('');
            return;
        }

        // Set timeout to avoid too many requests
        searchTimeout = setTimeout(function () {
            performSearch(query);
        }, 300);
    });

    // Focus event handler
    $searchInput.on('focus', function () {
        var query = $(this).val().trim();
        if (query.length >= 2 && $searchResults.html().trim().length > 0) {
            $searchResults.show();
        }
    });

    // Click outside to close results
    $(document).on('click', function (e) {
        if (!$(e.target).closest('.search-container').length) {
            $searchResults.hide();
        }
    });

    // Keep results open when clicking inside them
    $searchResults.on('click', function (e) {
        e.stopPropagation();
    });

    function performSearch(query) {
        console.log('Performing search for:', query); // Debug log

        $.ajax({
            url: '/Book/SearchBooks',
            type: 'GET',
            data: { searchTerm: query },
            success: function (response) {
                console.log('Search response received'); // Debug log
                $searchResults.html(response);

                if (response && response.trim().length > 0 && !response.includes('No books found')) {
                    $searchResults.show();
                } else {
                    $searchResults.hide();
                }
            },
            error: function (xhr, status, error) {
                console.error('Search error:', error);
                console.log('Status:', status);
                console.log('Response:', xhr.responseText);
                $searchResults.hide().html('<div class="no-results">Search temporarily unavailable</div>');
            }
        });
    }
});