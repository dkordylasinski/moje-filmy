<template>
	<li class="list-group-item">
		<div class="d-flex w-100 justify-content-between">
			<h3>{{ props.movie.title }} ({{ props.movie.year }})</h3>
			<div>
				<button
					type="button"
					class="btn btn-warning"
					@click="$emit('editMovie', props.movie.id)"
				>
					Edytuj
				</button>
				<button
					type="button"
					class="btn btn-danger ms-2"
					@click="$emit('deleteConfirm', props.movie.id)"
				>
					Usuń
				</button>
			</div>
		</div>
		<p
			v-if="props.movie.director"
			class="mb-1"
		>
			<strong>Reżyser:</strong> {{ props.movie.director }}
		</p>
		<p
			v-if="props.movie.description"
			class="mb-1"
		>
			<strong>Opis:</strong> {{ props.movie.description }}
		</p>
	</li>
</template>

<script setup>
	import { defineProps } from 'vue';

	const props = defineProps({
		movie: Object,
	});
</script>

<style lang="scss" scoped></style>
