<template>
	<div
		class="modal fade"
		id="errorModal"
		tabindex="-1"
		aria-labelledby="errorModalLabel"
		aria-hidden="true"
	>
		<div class="modal-dialog">
			<div class="modal-content">
				<div class="modal-header">
					<h5
						class="modal-title"
						id="errorModalLabel"
					>
						{{ errorTexts.title }}
					</h5>
					<button
						type="button"
						class="btn-close"
						data-bs-dismiss="modal"
						aria-label="Close"
					></button>
				</div>
				<div class="modal-body">
					{{ errorTexts.description }}
				</div>
				<div class="modal-footer">
					<button
						type="button"
						class="btn btn-secondary"
						data-bs-dismiss="modal"
					>
						Zamknij
					</button>
				</div>
			</div>
		</div>
	</div>
</template>

<script setup>
	import { inject, defineExpose, ref } from 'vue';
	const bootstrap = inject('bootstrap');

	const errorTexts = ref({
		title: '',
		description: '',
	});

	let modalInstance;
	const showErrorDialog = errorInfo => {
		if (errorInfo) {
			errorInfo.code
				? (errorTexts.value.title = errorInfo.code)
				: (errorTexts.value.title = 'Wystąpił błąd');
			errorInfo.message
				? (errorTexts.value.description = errorInfo.message)
				: (errorTexts.value.description =
						'Wystąpił niespodziewany błąd z aplikacją.');

			const modalElement = document.getElementById('errorModal');
			if (modalElement) {
				modalInstance = new bootstrap.Modal(modalElement);
				modalInstance.show();
			} else {
				console.error('Modal element not found');
			}
		}
	};

	defineExpose({
		showErrorDialog,
	});
</script>

<style lang="scss" scoped></style>
