// AJAX Search Engine
$(document).ready(function() {
    let searchTimeout;
    
    $('#searchInput').on('keyup', function() {
        clearTimeout(searchTimeout);
        
        const query = $(this).val().trim();
        
        if (query.length === 0) {
            $('#searchResults').removeClass('show');
            return;
        }
        
        // Debounce: Wait 300ms before making request
        searchTimeout = setTimeout(function() {
            $.ajax({
                url: '/Book/SearchBooks',
                type: 'GET',
                data: { query: query },
                dataType: 'json',
                success: function(data) {
                    displaySearchResults(data);
                },
                error: function(xhr, status, error) {
                    console.error('Search failed:', error);
                    console.log('Response:', xhr.responseText);
                }
            });
        }, 300);
    });
    
    // Hide results when clicking outside
    $(document).on('click', function(e) {
        if (!$(e.target).closest('#searchInput, #searchResults').length) {
            $('#searchResults').removeClass('show');
        }
    });
});

function displaySearchResults(books) {
    const resultsList = $('#resultsList');
    const searchResults = $('#searchResults');
    
    resultsList.empty();
    
    if (!books || books.length === 0) {
        resultsList.html('<li class="list-group-item text-muted text-center py-3">No books found</li>');
    } else {
        books.forEach(book => {
            const imageUrl = book.bookImage 
                ? '/' + book.bookImage 
                : 'https://placehold.co/40x60?text=No+Cover';
            
            const resultItem = `
                <li class="list-group-item">
                    <a href="/Book/Details/${book.bookId}" 
                       class="text-decoration-none text-dark">
                        <img src="${imageUrl}" 
                             alt="${book.title}"
                             onerror="this.src='https://placehold.co/40x60?text=No+Cover'">
                        <div>
                            <strong>${escapeHtml(book.title)}</strong>
                            <small class="d-block text-muted">${escapeHtml(book.authorName)}</small>
                            <small class="text-warning"><i class="fa-solid fa-star"></i> ${parseFloat(book.rate).toFixed(1)}</small>
                        </div>
                    </a>
                </li>
            `;
            resultsList.append(resultItem);
        });
    }
    
    // Position dropdown below search input
    const inputOffset = $('#searchInput').offset();
    const inputHeight = $('#searchInput').outerHeight();
    
    searchResults.css({
        'top': (inputOffset.top + inputHeight + 5) + 'px',
        'left': inputOffset.left + 'px'
    });
    
    searchResults.addClass('show');
}

function escapeHtml(text) {
    if (!text) return '';
    const map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };
    return text.replace(/[&<>"']/g, m => map[m]);
}