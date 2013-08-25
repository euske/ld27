#!/usr/bin/env python
import sys
import pygame

pygame.font.init()
img = pygame.Surface((100, 100))
img.fill((255,255,255,255))
(r0,g0,b0) = (16, 128, 252)
(r1,g1,b1) = (16, 16, 16)
for t in xrange(100):
    color = ((r1*t+r0*(100-t))/100,
             (g1*t+g0*(100-t))/100,
             (b1*t+b0*(100-t))/100)
    img.fill(color, (0, t, 100, 1))
pygame.image.save(img, 'sky.png')
