<template>
	<the-header></the-header>
	<div class="container">
		<movies-list></movies-list>
	</div>
</template>

<script setup>
	import TheHeader from './components/TheHeader.vue';
	import MoviesList from './components/movies-list/MoviesList.vue';
</script>

<style>
	.nav {
		width: 50%;
		margin: auto;
	}
</style>
