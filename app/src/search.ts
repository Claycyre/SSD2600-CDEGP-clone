const DEBOUNCE_MS = 300;

document.addEventListener("DOMContentLoaded", () => {
  const params = new URLSearchParams(window.location.search);
  const hasSearch = params.has("q");
  if (!hasSearch) return;

  const searchInput = document.querySelector(
    'input[name="q"]:not([type="hidden"])',
  ) as HTMLInputElement;

  searchInput.focus();
  searchInput.selectionEnd = searchInput.selectionStart =
    searchInput.value.length;
});

let debounceTimerHandle: number;
function searchDebounced(e: InputEvent) {
  const target = e.target as HTMLInputElement;
  const parentForm = target.form;
  if (!parentForm) return;

  if (debounceTimerHandle) clearTimeout(debounceTimerHandle);

  debounceTimerHandle = setTimeout(() => {
    parentForm.requestSubmit();
  }, DEBOUNCE_MS);
}
