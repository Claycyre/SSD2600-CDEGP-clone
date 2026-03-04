console.log("Loaded JS");

function scrollToElement(id: string, opts?: ScrollIntoViewOptions) {
  const element = document.getElementById(id);
  if (!element) return;

  element.scrollIntoView(opts);
}
