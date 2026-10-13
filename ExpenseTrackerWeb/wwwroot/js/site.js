(() => {
  "use strict";

  const app = document.getElementById("expenseApp");

  if (!app) {
    return;
  }

  const apiBaseUrl = app.dataset.apiBaseUrl;
  const currency = new Intl.NumberFormat("it-IT", { style: "currency", currency: "EUR" });
  const dateFormatter = new Intl.DateTimeFormat("it-IT", { day: "2-digit", month: "short", year: "numeric" });
  const monthFormatter = new Intl.DateTimeFormat("it-IT", { month: "long", year: "numeric" });
  const state = { expenses: [], breakdown: {}, categories: [] };

  const elements = {
    monthlyTotal: document.getElementById("monthlyTotal"),
    monthlyLabel: document.getElementById("monthlyLabel"),
    expenseCount: document.getElementById("expenseCount"),
    averageExpense: document.getElementById("averageExpense"),
    topCategory: document.getElementById("topCategory"),
    topCategoryAmount: document.getElementById("topCategoryAmount"),
    categoryBreakdown: document.getElementById("categoryBreakdown"),
    expenseTableBody: document.getElementById("expenseTableBody"),
    emptyState: document.getElementById("emptyState"),
    expenseSearch: document.getElementById("expenseSearch"),
    categoryFilter: document.getElementById("categoryFilter"),
    formCategory: document.getElementById("formCategory"),
    expenseModal: document.getElementById("expenseModal"),
    expenseForm: document.getElementById("expenseForm"),
    formError: document.getElementById("formError"),
    submitExpense: document.getElementById("submitExpense"),
    toast: document.getElementById("toast")
  };

  const categorySymbols = {
    Food: "FD",
    Transport: "TR",
    Entertainment: "EN",
    Shopping: "SH",
    Bills: "BL",
    Health: "HL",
    Education: "ED",
    Travel: "TV",
    "Personal Care": "PC",
    Home: "HM"
  };

  async function request(path, options) {
    const response = await fetch(`${apiBaseUrl}${path}`, {
      headers: { "Content-Type": "application/json", ...(options?.headers || {}) },
      ...options
    });

    if (!response.ok) {
      const payload = await response.json().catch(() => null);
      throw new Error(payload?.error || payload?.title || `Errore API (${response.status})`);
    }

    return response.status === 204 ? null : response.json();
  }

  async function loadDashboard() {
    try {
      const [expenses, summary, breakdown, categories] = await Promise.all([
        request("/Expenses"),
        request("/Expenses/monthly-summary"),
        request("/Expenses/category-breakdown"),
        request("/Expenses/categories")
      ]);

      state.expenses = expenses;
      state.breakdown = breakdown;
      state.categories = categories;
      renderSummary(summary);
      renderCategoryOptions();
      renderBreakdown();
      renderExpenses();
    } catch (error) {
      renderUnavailableState();
      showToast(`API non disponibile. Verifica che sia avviata su ${apiBaseUrl}.`, true);
      console.error(error);
    }
  }

  function renderSummary(summary) {
    const total = Number(summary.total);
    const average = state.expenses.length
      ? state.expenses.reduce((sum, expense) => sum + Number(expense.amount), 0) / state.expenses.length
      : 0;
    const sortedCategories = Object.entries(state.breakdown).sort(([, first], [, second]) => second - first);
    const [topCategory, topAmount] = sortedCategories[0] || [];

    elements.monthlyTotal.textContent = currency.format(total);
    elements.monthlyLabel.textContent = capitalize(monthFormatter.format(new Date(summary.year, summary.month - 1, 1)));
    elements.expenseCount.textContent = state.expenses.length.toString();
    elements.averageExpense.textContent = currency.format(average);
    elements.topCategory.textContent = topCategory || "—";
    elements.topCategoryAmount.textContent = topCategory ? currency.format(topAmount) : "Nessuna spesa";
  }

  function renderCategoryOptions() {
    const options = state.categories
      .map(category => `<option value="${escapeHtml(category)}">${escapeHtml(category)}</option>`)
      .join("");
    const currentFilter = elements.categoryFilter.value;

    elements.categoryFilter.innerHTML = `<option value="">Tutte le categorie</option>${options}`;
    elements.formCategory.innerHTML = `<option value="">Seleziona una categoria</option>${options}`;
    elements.categoryFilter.value = currentFilter;
  }

  function renderBreakdown() {
    const entries = Object.entries(state.breakdown).sort(([, first], [, second]) => second - first);
    const total = entries.reduce((sum, [, amount]) => sum + Number(amount), 0);

    if (!entries.length) {
      elements.categoryBreakdown.innerHTML = '<div class="category-empty">Le categorie appariranno dopo aver inserito una spesa.</div>';
      return;
    }

    elements.categoryBreakdown.innerHTML = entries.slice(0, 6).map(([category, amount]) => {
      const percentage = total ? Math.round((Number(amount) / total) * 100) : 0;
      return `
        <div class="category-item">
          <span class="category-dot">${escapeHtml(getCategorySymbol(category))}</span>
          <div class="category-copy">
            <span class="category-name">${escapeHtml(category)}</span>
            <div class="progress-track"><div class="progress-bar" style="width: ${percentage}%"></div></div>
          </div>
          <span class="category-amount">${currency.format(amount)}</span>
        </div>`;
    }).join("");
  }

  function renderExpenses() {
    const query = elements.expenseSearch.value.trim().toLocaleLowerCase("it");
    const category = elements.categoryFilter.value;
    const filtered = state.expenses
      .filter(expense => !category || expense.category === category)
      .filter(expense => !query || `${expense.description} ${expense.category}`.toLocaleLowerCase("it").includes(query))
      .sort((first, second) => second.date.localeCompare(first.date));

    elements.expenseTableBody.hidden = filtered.length === 0;
    elements.emptyState.hidden = filtered.length !== 0;
    elements.expenseTableBody.innerHTML = filtered.map(expense => `
      <tr>
        <td>
          <div class="description-cell">
            <span class="expense-symbol">${escapeHtml(getCategorySymbol(expense.category))}</span>
            <span>${escapeHtml(expense.description || "Spesa senza descrizione")}</span>
          </div>
        </td>
        <td><span class="category-pill">${escapeHtml(expense.category)}</span></td>
        <td>${dateFormatter.format(parseLocalDate(expense.date))}</td>
        <td class="expense-amount">${currency.format(expense.amount)}</td>
        <td>
          <button class="delete-button" type="button" data-delete-id="${expense.id}" aria-label="Elimina ${escapeHtml(expense.description || "spesa")}">
            <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M4 7h16M9 7V4h6v3M7 7l1 13h8l1-13M10 11v5M14 11v5" /></svg>
          </button>
        </td>
      </tr>`).join("");
  }

  function renderUnavailableState() {
    elements.categoryBreakdown.innerHTML = '<div class="category-empty">Impossibile caricare le categorie.</div>';
    elements.expenseTableBody.hidden = true;
    elements.emptyState.hidden = false;
    elements.emptyState.querySelector("h3").textContent = "API non disponibile";
    elements.emptyState.querySelector("p").textContent = `Avvia ExpenseTrackerAPI su ${apiBaseUrl} e ricarica la pagina.`;
  }

  function openModal() {
    elements.expenseForm.reset();
    elements.expenseForm.elements.date.value = toDateInputValue(new Date());
    elements.formError.hidden = true;
    elements.expenseModal.hidden = false;
    document.body.style.overflow = "hidden";
    window.setTimeout(() => elements.expenseForm.elements.description.focus(), 0);
  }

  function closeModal() {
    elements.expenseModal.hidden = true;
    document.body.style.overflow = "";
  }

  async function createExpense(event) {
    event.preventDefault();
    elements.submitExpense.disabled = true;
    elements.submitExpense.textContent = "Salvataggio...";
    elements.formError.hidden = true;

    const formData = new FormData(elements.expenseForm);
    const payload = {
      description: formData.get("description").trim(),
      amount: Number(formData.get("amount")),
      category: formData.get("category"),
      date: formData.get("date")
    };

    try {
      await request("/Expenses", { method: "POST", body: JSON.stringify(payload) });
      closeModal();
      showToast("Spesa aggiunta correttamente.");
      await loadDashboard();
    } catch (error) {
      elements.formError.textContent = error.message;
      elements.formError.hidden = false;
    } finally {
      elements.submitExpense.disabled = false;
      elements.submitExpense.textContent = "Salva spesa";
    }
  }

  async function deleteExpense(id) {
    if (!window.confirm("Vuoi eliminare questa spesa?")) {
      return;
    }

    try {
      await request(`/Expenses/${id}`, { method: "DELETE" });
      showToast("Spesa eliminata.");
      await loadDashboard();
    } catch (error) {
      showToast(error.message, true);
    }
  }

  function showToast(message, isError = false) {
    window.clearTimeout(showToast.timeoutId);
    elements.toast.textContent = message;
    elements.toast.classList.toggle("error", isError);
    elements.toast.hidden = false;
    showToast.timeoutId = window.setTimeout(() => {
      elements.toast.hidden = true;
    }, 4200);
  }

  function getCategorySymbol(category) {
    return categorySymbols[category] || category.slice(0, 2).toUpperCase();
  }

  function parseLocalDate(value) {
    const [year, month, day] = value.split("-").map(Number);
    return new Date(year, month - 1, day);
  }

  function toDateInputValue(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    return `${year}-${month}-${day}`;
  }

  function capitalize(value) {
    return value.charAt(0).toUpperCase() + value.slice(1);
  }

  function escapeHtml(value) {
    const element = document.createElement("span");
    element.textContent = String(value);
    return element.innerHTML;
  }

  document.querySelectorAll("[data-open-expense-modal]").forEach(button => button.addEventListener("click", openModal));
  document.querySelectorAll("[data-close-expense-modal]").forEach(button => button.addEventListener("click", closeModal));
  elements.expenseModal.addEventListener("click", event => {
    if (event.target === elements.expenseModal) {
      closeModal();
    }
  });
  elements.expenseForm.addEventListener("submit", createExpense);
  elements.expenseSearch.addEventListener("input", renderExpenses);
  elements.categoryFilter.addEventListener("change", renderExpenses);
  elements.expenseTableBody.addEventListener("click", event => {
    const deleteButton = event.target.closest("[data-delete-id]");
    if (deleteButton) {
      deleteExpense(deleteButton.dataset.deleteId);
    }
  });
  document.addEventListener("keydown", event => {
    if (event.key === "Escape" && !elements.expenseModal.hidden) {
      closeModal();
    }
  });

  loadDashboard();
})();
