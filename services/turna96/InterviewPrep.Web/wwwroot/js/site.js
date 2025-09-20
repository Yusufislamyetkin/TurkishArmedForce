(() => {
    const streakElement = document.querySelector('[data-streak-progress]');
    if (!streakElement) {
        return;
    }

    const streak = Number.parseInt(streakElement.dataset.streakProgress ?? '0', 10);
    const target = streakElement.querySelector('.progress-bar');
    if (!target) {
        return;
    }

    const normalized = Math.min(100, streak);
    target.style.width = `${normalized}%`;
})();
