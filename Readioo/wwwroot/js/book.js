// ============================================================
// assets/js/book.js
// ============================================================

// --- Generate Star SVG ---
function makeStarSVG(filled = true) {
    const color = filled ? '#f6b438' : '#e0e0e0';
    return `<svg viewBox="0 0 20 20" fill="${color}" xmlns="http://www.w3.org/2000/svg" style="width: 18px; height: 18px; display: inline-block;">
    <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.286 3.951a1 1 0 
    00.95.69h4.162c.969 0 1.371 1.24.588 1.81l-3.37 2.448a1 1 0 
    00-.364 1.118l1.286 3.951c.3.921-.755 1.688-1.538 
    1.118l-3.37-2.448a1 1 0 00-1.176 0l-3.37 
    2.448c-.783.57-1.838-.197-1.538-1.118l1.286-3.951a1 
    1 0 00-.364-1.118L2.063 9.378c-.783-.57-.38-1.81.588-1.81h4.162a1 
    1 0 00.95-.69l1.286-3.951z"/>
  </svg>`;
}

// --- Toggle Description ---
function setupDescriptionToggle() {
    const desc = document.getElementById("bookDesc");
    const toggle = document.getElementById("toggleDesc");

    if (!desc || !toggle) return;

    const fullText = desc.textContent;
    const shortText = fullText.slice(0, 180) + "...";

    let expanded = false;
    desc.textContent = shortText;

    toggle.addEventListener("click", () => {
        expanded = !expanded;
        desc.textContent = expanded ? fullText : shortText;
        toggle.textContent = expanded ? "Show less" : "Show more";
    });
}

// --- Main Initialization ---
document.addEventListener("DOMContentLoaded", () => {

    // 1. Render Average Rating Stars
    const avgRatingContainer = document.getElementById("avgRating");
    if (avgRatingContainer) {
        // avgRating value is set in the inline script in the view
        // This runs after that script, so we just need to ensure it's visible
    }

    // 2. Setup User Rating Interaction
    const userStarsContainer = document.getElementById("userStars");
    if (userStarsContainer) {
        const bookId = userStarsContainer.dataset.bookId;
        const currentRating = parseInt(userStarsContainer.dataset.userRating) || 0;
        const stars = userStarsContainer.querySelectorAll(".star-rate");

        // Highlight current rating on load
        highlightStars(currentRating);

        stars.forEach((star, index) => {
            // Hover effect
            star.addEventListener("mouseenter", function () {
                const rating = parseInt(this.dataset.rating);
                highlightStars(rating);
            });

            // Click to rate
            star.addEventListener("click", function () {
                const rating = parseInt(this.dataset.rating);
                submitRating(bookId, rating);
            });
        });

        // Reset on mouse leave
        userStarsContainer.addEventListener("mouseleave", function () {
            highlightStars(currentRating);
        });

        function highlightStars(rating) {
            stars.forEach((star, index) => {
                if (index < rating) {
                    star.classList.add("filled");
                } else {
                    star.classList.remove("filled");
                }
            });
        }

        function submitRating(bookId, rating) {
            // Get anti-forgery token if you're using it
            const token = document.querySelector('input[name="__RequestVerificationToken"]');

            fetch(`/Book/RateBook`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: `bookId=${bookId}&rating=${rating}`
            })
                .then(response => {
                    if (response.ok) {
                        userStarsContainer.dataset.userRating = rating;
                        location.reload(); // Reload to show updated average rating
                    } else {
                        alert('Failed to save rating. Please try again.');
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred while saving your rating.');
                });
        }
    }

    // 3. Setup Description Toggle
    setupDescriptionToggle();
});