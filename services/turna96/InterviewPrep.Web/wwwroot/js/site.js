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

(() => {
    const root = document.querySelector('[data-practice-root]');
    if (!root) {
        return;
    }

    const practiceId = root.getAttribute('data-practice-id');
    const scoreElement = document.querySelector('[data-practice-score]');
    let totalScore = Number(scoreElement?.textContent ?? '0');

    const showMessage = (message, icon = 'info') => {
        if (typeof Swal === 'undefined') {
            if (icon === 'error') {
                alert(message);
            } else {
                console.log(message);
            }
            return;
        }

        Swal.fire({
            icon,
            text: message,
            confirmButtonText: 'Tamam'
        });
    };

    const cards = root.querySelectorAll('[data-question-card]');
    cards.forEach((card) => {
        const button = card.querySelector('[data-submit-question]');
        if (!button) {
            return;
        }

        button.addEventListener('click', async () => {
            if (!practiceId) {
                showMessage('Oturum bilgisi bulunamadı.', true);
                return;
            }

            const questionId = card.getAttribute('data-question-id');
            if (!questionId) {
                showMessage('Soru kimliği bulunamadı.', true);
                return;
            }

            const type = card.getAttribute('data-question-type');
            const payload = {
                practiceId,
                questionId,
                answer: null,
                selectedOptionId: null
            };

            if (type === 'MultipleChoice') {
                const selected = card.querySelector('[data-option-input]:checked');
                if (!selected) {
                    showMessage('Önce bir seçenek seçmelisin.', true);
                    return;
                }

                payload.selectedOptionId = selected.getAttribute('value');
            } else {
                const answerField = card.querySelector('[data-answer-input]');
                if (!answerField || !(answerField instanceof HTMLTextAreaElement || answerField instanceof HTMLInputElement)) {
                    showMessage('Yanıt alanı bulunamadı.', true);
                    return;
                }

                payload.answer = answerField.value;
            }

            button.setAttribute('disabled', 'disabled');
            button.classList.add('disabled');

            try {
                const response = await fetch('/Questions/Evaluate', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(payload)
                });

                if (!response.ok) {
                    const errorData = await response.json().catch(() => ({ message: 'Yanıt değerlendirilemedi.' }));
                    showMessage(errorData.message ?? 'Yanıt değerlendirilemedi.', 'error');
                    return;
                }

                const data = await response.json();

                if (data.correct && !data.alreadyCompleted) {
                    totalScore += Number(data.score ?? 0);
                    if (scoreElement) {
                        scoreElement.textContent = totalScore.toString();
                    }
                }

                if (data.correct) {
                    card.classList.remove('border-secondary');
                    card.classList.add('border-success');
                    button.classList.remove('btn-outline-success');
                    button.classList.add('btn-success');
                    button.innerHTML = '<i class="bi bi-check-circle"></i> Doğru!';
                    showMessage(data.message ?? 'Yanıt değerlendirildi.', data.alreadyCompleted ? 'info' : 'success');
                } else {
                    card.classList.add('shake');
                    setTimeout(() => card.classList.remove('shake'), 500);
                    button.removeAttribute('disabled');
                    button.classList.remove('disabled');
                    showMessage(data.message ?? 'Yanıt değerlendirildi.', data.alreadyCompleted ? 'info' : 'error');
                }
            } catch (error) {
                console.error(error);
                showMessage('Cevap kontrol edilirken bir hata meydana geldi.', 'error');
                button.removeAttribute('disabled');
                button.classList.remove('disabled');
            }
        });
    });
})();
