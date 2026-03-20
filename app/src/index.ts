console.log("Loaded JS");

function scrollToElement(id: string, opts?: ScrollIntoViewOptions) {
  const element = document.getElementById(id);
  if (!element) return;

  element.scrollIntoView(opts);
}

document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("[data-scroll-target]").forEach((el) => {
    el.addEventListener("click", () => {
      const id = el.getAttribute("data-scroll-target");
      scrollToElement(id, { behavior: "smooth" });
    });
  });
});
