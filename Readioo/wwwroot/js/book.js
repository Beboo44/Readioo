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
  const container = document.getElementById("userStars");
  let currentRating = 0;

  function updateStars() {
    container.innerHTML = Array.from({ length: 5 }, (_, i) =>
      makeStarSVG(i < currentRating)
    ).join('');
    container.querySelectorAll("svg").forEach((svg, idx) => {
      svg.addEventListener("click", () => {
        currentRating = idx + 1;
        updateStars();
      });
    });
  }

  updateStars();
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

// --- Load Mock Reviews ---
function loadReviews() {
  const reviews = [
    { user: "Sarah", rating: 5, text: "A haunting masterpiece that explores the consequences of creation and rejection." },
    { user: "Ahmed", rating: 4, text: "Beautifully written and deeply philosophical. A timeless classic." },
    { user: "Mina", rating: 3, text: "Interesting ideas, but the pacing felt slow in the middle chapters." }
  ];

  const container = document.getElementById("reviewsContainer");
  reviews.forEach(r => {
    const div = document.createElement("div");
    div.className = "review-item";
    const stars = Array.from({ length: 5 }, (_, i) => makeStarSVG(i < r.rating)).join('');
    div.innerHTML = `
      <div class="review-user">${r.user}</div>
      <div class="review-rating">${stars}</div>
      <p>${r.text}</p>
    `;
    container.appendChild(div);
  });
}

// --- Init ---
document.addEventListener("DOMContentLoaded", () => {
  renderAvgRating();
  renderUserRating();
  setupDescriptionToggle();
  loadReviews();
});
