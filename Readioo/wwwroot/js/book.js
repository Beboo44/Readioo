// ============================================================
// assets/js/book.js
// ============================================================

// --- Generate Star SVG ---
function makeStarSVG(filled = true) {
  const color = filled ? '#f6b438' : '#e0e0e0';
  return `<svg viewBox="0 0 20 20" fill="${color}" xmlns="http://www.w3.org/2000/svg">
    <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.286 3.951a1 1 0 
    00.95.69h4.162c.969 0 1.371 1.24.588 1.81l-3.37 2.448a1 1 0 
    00-.364 1.118l1.286 3.951c.3.921-.755 1.688-1.538 
    1.118l-3.37-2.448a1 1 0 00-1.176 0l-3.37 
    2.448c-.783.57-1.838-.197-1.538-1.118l1.286-3.951a1 
    1 0 00-.364-1.118L2.063 9.378c-.783-.57-.38-1.81.588-1.81h4.162a1 
    1 0 00.95-.69l1.286-3.951z"/>
  </svg>`;
}

// --- Render Average Rating ---
function renderAvgRating() {
  const avgStars = 4; // example static avg rating
  const container = document.getElementById("avgRating");
  container.innerHTML = Array.from({ length: 5 }, (_, i) =>
    makeStarSVG(i < avgStars)
  ).join('');
}

// --- Handle User Rating ---
function renderUserRating() {
    // 1. Target the visual stars in the header section
    const visualContainer = document.getElementById("userStars");

    // 2. Target the radio buttons in the review form
    const formRatingInputs = document.querySelectorAll('.gr-rating-input input[name="Rating"]');

    let currentRating = 0;

    function updateVisualStars(rating) {
        visualContainer.innerHTML = Array.from({ length: 5 }, (_, i) =>
            makeStarSVG(i < rating)
        ).join('');

        // Add click handlers to the new visual stars
        visualContainer.querySelectorAll("svg").forEach((svg, idx) => {
            svg.addEventListener("click", () => {
                currentRating = idx + 1;
                // Update both visual stars and form inputs
                updateVisualStars(currentRating);
                updateFormRating(currentRating);
            });
        });
    }

    function updateFormRating(rating) {
        // Check the radio button in the actual form that matches the rating
        formRatingInputs.forEach(input => {
            input.checked = (parseInt(input.value) === rating);
        });
    }

    // Initialize stars (optional: can be loaded from model if user has rated before)
    updateVisualStars(currentRating);

    // Optional: Add event listener to the form's radio buttons 
    // to update the visual stars if the user rates using the form directly
    formRatingInputs.forEach(input => {
        input.addEventListener('change', (e) => {
            currentRating = parseInt(e.target.value);
            updateVisualStars(currentRating);
        });
    });
}

// --- Toggle Description ---
function setupDescriptionToggle() {
  const desc = document.getElementById("bookDesc");
  const toggle = document.getElementById("toggleDesc");
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


document.addEventListener("DOMContentLoaded", () => {
  renderAvgRating();
  renderUserRating();
  setupDescriptionToggle();
  
});
