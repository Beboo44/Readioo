document.addEventListener("DOMContentLoaded", () => {
  renderFavBooks();

  const followBtn = document.querySelector(".follow-btn");
  followBtn.addEventListener("click", () => {
    if (followBtn.classList.contains("following")) {
      followBtn.classList.remove("following");
      followBtn.innerHTML = '<i class="fa-solid fa-user-plus"></i> Follow';
    } else {
      followBtn.classList.add("following");
      followBtn.innerHTML = '<i class="fa-solid fa-check"></i> Following';
    }
  });
});
